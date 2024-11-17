using MediatR;
using ProjectManager.Core.Entities;
using ProjectManager.Modules.Projects.Contracts.Requests;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Projects.Features.Commands;

public class CreateProjectCommand(ProjectDbContext dbContext) : IRequestHandler<CreateProjectRequest, int>
{
    public async Task<int> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var newProject = new Project
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate
        };

        dbContext.Projects.Add(newProject);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newProject.Id;
    }
}
