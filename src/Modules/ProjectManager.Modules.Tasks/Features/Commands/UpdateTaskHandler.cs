using Ardalis.Result;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Contracts.Responses;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Tasks.Features.Commands;

// public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
// {
//     public UpdateTaskValidator()
//     {
//         RuleFor(t => t.Id).GreaterThan(0);
//         RuleFor(t => t.Name).NotEmpty().MaximumLength(100);
//         RuleFor(t => t.Description).MaximumLength(500);
//         RuleFor(t => t.AssigneeId).NotEmpty();
//     }
// }

public class UpdateTaskHandler(ProjectDbContext dbContext) : IRequestHandler<UpdateTaskRequest, Result<TaskResponse>>
{
    public async Task<Result<TaskResponse>> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await dbContext.Tasks.FindAsync([request.Id], cancellationToken);
        
        if (task is null)
        {
            return Result.NotFound($"Task with ID {request.Id} not found.");
        }
        
        if (!await dbContext.Users.AnyAsync(u => u.Id == request.AssigneeId, cancellationToken))
        {
            return Result.NotFound($"Assignee with id {request.AssigneeId} not exists");
        }

        task.Name = request.Name;
        task.Description = request.Description;
        task.Deadline = request.Deadline;
        task.CurrentStatus = request.CurrentStatus;
        task.Priority = request.Priority;
        task.AssigneeId = request.AssigneeId;
        task.UpdatedDate = DateTime.UtcNow;
        
        dbContext.Update(task);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new TaskResponse
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            CreatedDate = task.CreatedDate,
            UpdatedDate = task.UpdatedDate,
            Deadline = task.Deadline,
            CreatorId = task.CreatorId,
            AssigneeId = task.AssigneeId,
            BoardId = task.BoardId,
            CurrentStatus = task.CurrentStatus,
            Priority = task.Priority
        });
    }
}