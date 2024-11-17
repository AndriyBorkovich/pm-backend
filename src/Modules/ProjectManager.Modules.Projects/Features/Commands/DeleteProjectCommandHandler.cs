using Ardalis.Result;
using MediatR;
using ProjectManager.Modules.Projects.Contracts.Requests;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Projects.Features.Commands;

public class DeleteProjectCommandHandler(ProjectDbContext dbContext) : IRequestHandler<DeleteProjectRequest, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects.FindAsync([request.Id], cancellationToken);
        if (project is null)
            return Result.NotFound($"Project with ID {request.Id} not found.");

        dbContext.Remove(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success($"Project with ID {request.Id} was deleted successfully");
    }
}
