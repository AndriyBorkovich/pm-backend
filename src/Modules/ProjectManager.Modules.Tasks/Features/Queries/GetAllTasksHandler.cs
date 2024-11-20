using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Contracts.Responses;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Tasks.Features.Queries;

public class GetAllTasksHandler(ProjectDbContext dbContext) : IRequestHandler<GetAllTasksRequest, Result<List<TaskResponse>>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetAllTasksRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Tasks.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.AssigneeId))
        {
            query = query.Where(t => t.AssigneeId == request.AssigneeId);
        }

        if (request.Status != null)
        {
            query = query.Where(t => t.CurrentStatus == request.Status);
        }

        if (request.Priority != null)
        {
            query = query.Where(t => t.Priority == request.Priority);
        }
        
        if (request.CreatedAfter.HasValue)
        {
            query = query.Where(t => t.CreatedDate >= request.CreatedAfter.Value);
        }

        if (request.CreatedBefore.HasValue)
        {
            query = query.Where(t => t.CreatedDate <= request.CreatedBefore.Value);
        }

        var tasks = await query.AsNoTracking().Select(t => new TaskResponse
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            CreatedDate = t.CreatedDate,
            UpdatedDate = t.UpdatedDate,
            Deadline = t.Deadline,
            CurrentStatus = t.CurrentStatus,
            Priority = t.Priority,
            BoardId = t.BoardId,
            CreatorId = t.CreatorId,
            AssigneeId = t.AssigneeId
        }).ToListAsync(cancellationToken);

        return Result.Success(tasks);
    }
}