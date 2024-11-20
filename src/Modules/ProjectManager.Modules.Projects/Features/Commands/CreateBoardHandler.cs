using Ardalis.Result;
using MediatR;
using ProjectManager.Core.Entities;
using ProjectManager.Persistence.Context;
using Task = System.Threading.Tasks.Task;

namespace ProjectManager.Modules.Projects.Features.Commands;

public class CreateBoardRequest : IRequest<Result<int>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
}

public class CreateBoardHandler(ProjectDbContext dbContext) : IRequestHandler<CreateBoardRequest, Result<int>>
{
    public async Task<Result<int>> Handle(CreateBoardRequest request, CancellationToken cancellationToken)
    {
        var newBoard = new Board
        {
            Name = request.Name,
            Description = request.Description,
            ProjectId = request.ProjectId
        };

        dbContext.Boards.Add(newBoard);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(newBoard.Id);
    }
}