using Ardalis.Result;
using MediatR;
using ProjectManager.Modules.Projects.Contracts.Requests;
using ProjectManager.Modules.Projects.Contracts.Responses;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Projects.Features.Commands;

public class UpdateProjectCommandHandler(ProjectDbContext dbContext) : IRequestHandler<UpdateProjectRequest, Result<ProjectResponse>>
{
    public async Task<Result<ProjectResponse>> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects.FindAsync([request.Id], cancellationToken);
        if (project is null)
            return Result.NotFound($"Project with ID {request.Id} not found.");

        project.Name = request.Name;
        project.Description = request.Description;
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;

        dbContext.Update(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new ProjectResponse(project.Id, project.Name, project.Description, project.StartDate, project.EndDate));
    }
}
