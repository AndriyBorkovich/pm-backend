using Ardalis.Result;
using MediatR;

namespace ProjectManager.Modules.Projects.Contracts.Requests
{
    public record DeleteProjectRequest(int Id) : IRequest<Result<string>>;
}
