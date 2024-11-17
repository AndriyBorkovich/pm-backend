using Ardalis.Result;
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
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project is null)
            {
                return Result.NotFound($"Project with ID {request.Id} not found.");
            }

            var response = new ProjectResponse(
                project.Id,
                project.Name,
                project.Description,
                project.StartDate,
                project.EndDate
            );

            return Result.Success(response);
        }
    }
}
