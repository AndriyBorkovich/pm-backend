using ProjectManager.Core.Entities;
using ProjectManager.Core.Enums;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Features.Queries;
using ProjectManager.Persistence.Context;
using SystemTask = System.Threading.Tasks.Task;
using TaskStatus = ProjectManager.Core.Enums.TaskStatus;

namespace ProjectManager.UnitTests.Task;

public class GetAllTasksHandlerTests(TestDbContextFixture fixture) : IClassFixture<TestDbContextFixture>, IAsyncLifetime
{
    private readonly ProjectDbContext _context = fixture.DbContext;

    public SystemTask InitializeAsync() => SeedDataAsync();

    public SystemTask DisposeAsync()
    {
        _context.ChangeTracker.Clear();
        _context.Database.EnsureDeleted();
        return SystemTask.CompletedTask;
    }
    
    private async SystemTask SeedDataAsync()
    {
        // Seed Users
        var users = new[]
        {
            new User { Id = "1", UserName = "User 1" },
            new User { Id = "2", UserName = "User 2" },
            new User { Id = "3", UserName = "User 3" }
        };
        await _context.Users.AddRangeAsync(users);

        // Seed Tasks
        var tasks = new[]
        {
            new Core.Entities.Task
            {
                Id = 1,
                Name = "Task 1",
                Description = "Task 1 Description",
                AssigneeId = "1",
                CurrentStatus = TaskStatus.ToDo,
                Priority = Priority.High,
                CreatedDate = DateTime.UtcNow.AddMinutes(-30)
            },
            new Core.Entities.Task
            {
                Id = 2,
                Name = "Task 2",
                Description = "Task 2 Description",
                AssigneeId = "1",
                CurrentStatus = TaskStatus.Done,
                Priority = Priority.Low,
                CreatedDate = DateTime.UtcNow.AddMinutes(-20)
            },
            new Core.Entities.Task
            {
                Id = 3,
                Name = "Task 3",
                Description = "Task 3 Description",
                AssigneeId = "2",
                CurrentStatus = TaskStatus.InProgress,
                Priority = Priority.Medium,
                CreatedDate = DateTime.UtcNow.AddMinutes(-10)
            }
        };
        await _context.Tasks.AddRangeAsync(tasks);

        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async SystemTask Handle_ReturnsAllTasks_WhenNoFilterApplied()
    {
        // Arrange
        var request = new GetAllTasksRequest();
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count);
    }

    [Fact]
    public async SystemTask Handle_ReturnsTasksFilteredByAssignee()
    {
        // Arrange
        var request = new GetAllTasksRequest { AssigneeId = "1" };
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.All(result.Value, t => Assert.Equal("1", t.AssigneeId));
    }

    [Fact]
    public async SystemTask Handle_ReturnsTasksFilteredByStatus()
    {
        // Arrange
        var request = new GetAllTasksRequest { Status = TaskStatus.ToDo };
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.All(result.Value, t => Assert.Equal(TaskStatus.ToDo, t.CurrentStatus));
    }

    [Fact]
    public async SystemTask Handle_ReturnsTasksFilteredByPriority()
    {
        // Arrange
        var request = new GetAllTasksRequest { Priority = Priority.High };
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.All(result.Value, t => Assert.Equal(Priority.High, t.Priority));
    }
    
    
    [Fact]
    public async SystemTask Handle_ReturnsTasksFilteredByCreationDate_After()
    {
        // Arrange
        var request = new GetAllTasksRequest { CreatedAfter = DateTime.UtcNow.AddDays(-1) };
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count);
    }

    [Fact]
    public async SystemTask Handle_ReturnsTasksFilteredByCreationDate_Before()
    {
        // Arrange
        var request = new GetAllTasksRequest { CreatedBefore = DateTime.UtcNow.AddDays(1) };
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count);
    }

    [Fact]
    public async SystemTask Handle_ReturnsTasksFilteredByCreationDate_Range()
    {
        // Arrange
        var request = new GetAllTasksRequest 
        { 
            CreatedAfter = DateTime.UtcNow.AddDays(-1),
            CreatedBefore = DateTime.UtcNow.AddDays(1)
        };
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count);
    }

    [Fact]
    public async SystemTask Handle_ReturnsTasksFilteredByMultipleCriteria()
    {
        // Arrange
        var request = new GetAllTasksRequest 
        { 
            AssigneeId = "1", 
            Status = TaskStatus.Done, 
            Priority = Priority.Low,
            CreatedAfter = DateTime.UtcNow.AddDays(-1),
            CreatedBefore = DateTime.UtcNow.AddDays(1)
        };
        var handler = new GetAllTasksHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        var task = result.Value.First();
        Assert.Equal("1", task.AssigneeId);
        Assert.Equal(TaskStatus.Done, task.CurrentStatus);
        Assert.Equal(Priority.Low, task.Priority);
    }
}