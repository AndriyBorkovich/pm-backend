using Ardalis.Result;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Core.Enums;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Contracts.Responses;
using ProjectManager.Persistence.Context;
using TaskStatus = ProjectManager.Core.Enums.TaskStatus;
using TaskEntity = ProjectManager.Core.Entities.Task;

namespace ProjectManager.Modules.Tasks.Features.Commands;

public class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskValidator()
    {
        RuleFor(t => t.Name).NotEmpty().MaximumLength(100);
        RuleFor(t => t.Description).MaximumLength(500);
        RuleFor(t => t.CreatorId).NotEmpty();
        RuleFor(t => t.BoardId).GreaterThan(0);
    }
}

public class CreateTaskHandler(ProjectDbContext dbContext) : IRequestHandler<CreateTaskRequest, Result<TaskResponse>>
{
    public async Task<Result<TaskResponse>> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (!await dbContext.Users.AnyAsync(u => u.Id == request.AssigneeId, cancellationToken))
        {
            return Result.NotFound($"Assignee with id {request.AssigneeId} not exists");
        }
        
        if (!await dbContext.Users.AnyAsync(u => u.Id == request.CreatorId, cancellationToken))
        {
            return Result.NotFound($"Creator with id {request.CreatorId} not exists");
        }

        if (!await dbContext.Boards.AnyAsync(b => b.Id == request.BoardId, cancellationToken))
        {
            return Result.NotFound($"Board with id {request.BoardId} not exists");
        }
        
        var task = new TaskEntity
        {
            Name = request.Name,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow,
            Deadline = request.Deadline,
            CreatorId = request.CreatorId,
            AssigneeId = request.AssigneeId,
            BoardId = request.BoardId,
            CurrentStatus = TaskStatus.ToDo,
            Priority = Priority.Medium
        };

        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new TaskResponse
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            CreatedDate = task.CreatedDate,
            Deadline = task.Deadline,
            CreatorId = task.CreatorId,
            AssigneeId = task.AssigneeId,
            BoardId = task.BoardId,
            CurrentStatus = task.CurrentStatus,
            Priority = task.Priority
        });
    }
}