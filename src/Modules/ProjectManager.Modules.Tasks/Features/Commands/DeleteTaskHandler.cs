using Ardalis.Result;
using MediatR;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Modules.Tasks.Features.Commands;


public class DeleteTaskHandler(ProjectDbContext dbContext) : IRequestHandler<DeleteTaskRequest, Result>
{
    public async Task<Result> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await dbContext.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);

        if (task == null)
        {
            return Result.NotFound($"Task with ID {request.Id} not found.");
        }

        dbContext.Tasks.Remove(task);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}