using Ardalis.Result;
using ProjectManager.Core.Entities;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Features.Commands;
using ProjectManager.Persistence.Context;
using SystemTask = System.Threading.Tasks.Task;
using Priority = ProjectManager.Core.Enums.Priority;
using TaskEntity = ProjectManager.Core.Entities.Task;
using TaskStatus = ProjectManager.Core.Enums.TaskStatus;

namespace ProjectManager.UnitTests.Task;

public class UpdateTaskHandlerTests(TestDbContextFixture fixture) : IClassFixture<TestDbContextFixture>, IAsyncLifetime
{
    private readonly ProjectDbContext _context = fixture.DbContext;

    public SystemTask InitializeAsync() => SystemTask.CompletedTask;

    public SystemTask DisposeAsync()
    {
        _context.ChangeTracker.Clear();
        return SystemTask.CompletedTask;
    }

    [Fact]
    public async SystemTask Handle_ReturnsSuccess_WhenTaskIsUpdated()
    {
        // Arrange
        var users = new[]
        {
            new User { Id = "66", UserName = "User 1" },
            new User { Id = "5", UserName = "User 2" }
        };
        await _context.Users.AddRangeAsync(users);

        var tasks = new[]
        {
            new TaskEntity
            {
                Id = 7,
                Name = "Task 1",
                Description = "Task Description 1",
                AssigneeId = "4",
                CurrentStatus = TaskStatus.ToDo,
                Deadline = DateTime.UtcNow.AddDays(10),
                Priority = Priority.High
            }
        };
        await _context.Tasks.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();

        var request = new UpdateTaskRequest
        {
            Id = 7,
            Name = "Updated Task 1",
            Description = "Updated Description",
            AssigneeId = "5",
            Deadline = DateTime.UtcNow.AddDays(5),
            CurrentStatus = TaskStatus.InProgress,
            Priority = Priority.Low
        };

        var handler = new UpdateTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(request.Name, result.Value.Name);
        Assert.Equal(request.Description, result.Value.Description);
        Assert.Equal(request.Deadline, result.Value.Deadline);
        Assert.Equal(request.CurrentStatus, result.Value.CurrentStatus);
        Assert.Equal(request.Priority, result.Value.Priority);
        Assert.Equal(request.AssigneeId, result.Value.AssigneeId);
    }

    [Fact]
    public async SystemTask Handle_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var users = new[]
        {
            new User { Id = "11", UserName = "User 1" }
        };
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        var request = new UpdateTaskRequest
        {
            Id = 99, // Non-existent Task ID
            Name = "Non-existent Task",
            Description = "Non-existent Description",
            AssigneeId = "11",
            Deadline = DateTime.UtcNow.AddDays(5),
            CurrentStatus = TaskStatus.InProgress,
            Priority = Priority.Low
        };
        var handler = new UpdateTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains("Task with ID 99 not found.", result.Errors);
    }

    [Fact]
    public async SystemTask Handle_ReturnsNotFound_WhenAssigneeDoesNotExist()
    {
        // Arrange
        var users = new[]
        {
            new User { Id = "4", UserName = "User 1" }
        };
        await _context.Users.AddRangeAsync(users);

        var tasks = new[]
        {
            new TaskEntity
            {
                Id = 5,
                Name = "Task 1",
                Description = "Task Description 1",
                AssigneeId = "4",
                CurrentStatus = TaskStatus.ToDo,
                Deadline = DateTime.UtcNow.AddDays(10),
                Priority = Priority.High
            }
        };
        await _context.Tasks.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();

        var request = new UpdateTaskRequest
        {
            Id = 5,
            Name = "Updated Task 1",
            Description = "Updated Description",
            AssigneeId = "99", // Non-existent assignee
            Deadline = DateTime.UtcNow.AddDays(5),
            CurrentStatus = TaskStatus.InProgress,
            Priority = Priority.Low
        };

        var handler = new UpdateTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains("Assignee with id 99 not exists", result.Errors);
    }
}
