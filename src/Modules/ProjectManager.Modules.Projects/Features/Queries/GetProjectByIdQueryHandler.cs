using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Modules.Projects.Contracts.Responses;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Projects.Features.Queries
{
    public record GetProjectByIdQuery: IRequest<Result<ProjectResponse>>
    {
        public int Id { get; set; }
    }

    public class GetProjectByIdQueryHandler(ProjectDbContext dbContext) : IRequestHandler<GetProjectByIdQuery, Result<ProjectResponse>>
    {
        public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects
                .Select(p => new ProjectResponse(p.Id, p.Name, p.Description, p.StartDate, p.EndDate))
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (project is null)
                return Result.NotFound($"Project with ID {request.Id} not found.");

            return Result.Success(project);
        }
    }
}
