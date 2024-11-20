using Ardalis.Result;
using ProjectManager.Core.Entities;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Features.Commands;
using ProjectManager.Persistence.Context;
using SystemTask = System.Threading.Tasks.Task;

namespace ProjectManager.UnitTests.Task;

public class CreateTaskHandlerTests(TestDbContextFixture fixture) : IClassFixture<TestDbContextFixture>, IAsyncLifetime
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
    public async SystemTask Handle_ReturnsSuccess_WhenTaskIsValid()
    {
        // Arrange
        var creator = new User { Id = "1", UserName = "User 1" };
        var assignee = new User { Id = "2", UserName = "User 2" };
        var board = new Board { Id = 1, Name = "Board 1", Description = "Board Description" };

        await _context.Users.AddRangeAsync(creator, assignee);
        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();

        var request = new CreateTaskRequest
        {
            Name = "New Task",
            Description = "New Task Description",
            CreatorId = creator.Id,
            AssigneeId = assignee.Id,
            BoardId = board.Id,
            Deadline = DateTime.UtcNow.AddDays(7)
        };
        var handler = new CreateTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(request.Name, result.Value.Name);
        Assert.Equal(request.Description, result.Value.Description);
        Assert.Equal(request.CreatorId, result.Value.CreatorId);
        Assert.Equal(request.AssigneeId, result.Value.AssigneeId);
        Assert.Equal(request.BoardId, result.Value.BoardId);
    }

    [Fact]
    public async SystemTask Handle_ReturnsNotFound_WhenAssigneeDoesNotExist()
    {
        // Arrange
        var creator = new User { Id = "3", UserName = "User 3" };
        var board = new Board { Id = 2, Name = "Board 1", Description = "Board Description" };

        await _context.Users.AddAsync(creator);
        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();

        var request = new CreateTaskRequest
        {
            Name = "New Task",
            Description = "New Task Description",
            CreatorId = creator.Id,
            AssigneeId = "99", // Non-existent assignee
            BoardId = board.Id,
            Deadline = DateTime.UtcNow.AddDays(7)
        };
        var handler = new CreateTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains("Assignee with id 99 not exists", result.Errors);
    }

    [Fact]
    public async SystemTask Handle_ReturnsNotFound_WhenCreatorDoesNotExist()
    {
        // Arrange
        var assignee = new User { Id = "4", UserName = "User 3" };
        var board = new Board { Id = 3, Name = "Board 1", Description = "Board Description" };

        await _context.Users.AddAsync(assignee);
        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();

        var request = new CreateTaskRequest
        {
            Name = "New Task",
            Description = "New Task Description",
            CreatorId = "99", // Non-existent creator
            AssigneeId = assignee.Id,
            BoardId = board.Id,
            Deadline = DateTime.UtcNow.AddDays(7)
        };
        var handler = new CreateTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains("Creator with id 99 not exists", result.Errors);
    }

    [Fact]
    public async SystemTask Handle_ReturnsNotFound_WhenBoardDoesNotExist()
    {
        // Arrange
        var creator = new User { Id = "6", UserName = "User 1" };
        var assignee = new User { Id = "7", UserName = "User 2" };

        await _context.Users.AddRangeAsync(creator, assignee);
        await _context.SaveChangesAsync();

        var request = new CreateTaskRequest
        {
            Name = "New Task",
            Description = "New Task Description",
            CreatorId = creator.Id,
            AssigneeId = assignee.Id,
            BoardId = 99, // Non-existent board
            Deadline = DateTime.UtcNow.AddDays(7)
        };
        var handler = new CreateTaskHandler(_context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains("Board with id 99 not exists", result.Errors);
    }
}
