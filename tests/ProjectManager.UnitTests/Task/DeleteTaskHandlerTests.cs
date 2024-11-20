using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Features.Commands;
using ProjectManager.Persistence.Context;
using TaskEntity = ProjectManager.Core.Entities.Task;
using SystemTask = System.Threading.Tasks.Task;
using Ardalis.Result;

namespace ProjectManager.UnitTests.Task;

public class DeleteTaskHandlerTests(TestDbContextFixture fixture) : IClassFixture<TestDbContextFixture>, IAsyncLifetime
{
    private readonly ProjectDbContext _context = fixture.DbContext;

    public SystemTask InitializeAsync() => SystemTask.CompletedTask;

    public SystemTask DisposeAsync()
    {
        _context.ChangeTracker.Clear();
        _context.Database.EnsureDeleted();
        return SystemTask.CompletedTask;
    }
    
    [Fact]
    public async SystemTask Handle_ReturnsSuccess_WhenTaskIsDeleted()
    {
        // Arrange
        var task = new TaskEntity { Id = 100, Name = "Task to be deleted", Description = "Description", AssigneeId = "1", BoardId = 1, CreatorId = "1", CurrentStatus = ProjectManager.Core.Enums.TaskStatus.ToDo, Priority = ProjectManager.Core.Enums.Priority.High, CreatedDate = DateTime.UtcNow };
        
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        var request = new DeleteTaskRequest { Id = task.Id };
        var handler = new DeleteTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(await _context.Tasks.FindAsync(task.Id));
    }
    
    [Fact]
    public async SystemTask Handle_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var request = new DeleteTaskRequest { Id = 99 };
        var handler = new DeleteTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains("Task with ID 99 not found.", result.Errors);
    }
}