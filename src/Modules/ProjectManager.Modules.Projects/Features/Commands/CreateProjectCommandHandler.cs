using Ardalis.Result;
using MediatR;
using ProjectManager.Core.Entities;
using ProjectManager.Modules.Projects.Contracts.Requests;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Projects.Features.Commands;

public class CreateProjectCommandHandler(ProjectDbContext dbContext) : IRequestHandler<CreateProjectRequest, Result<int>>
{
    public async Task<Result<int>> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var newProject = new Project
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        dbContext.Projects.Add(newProject);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(newProject.Id);
    }
}
