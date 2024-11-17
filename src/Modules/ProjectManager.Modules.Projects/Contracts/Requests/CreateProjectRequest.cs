using Ardalis.Result;
using MediatR;

namespace ProjectManager.Modules.Projects.Contracts.Requests;

public record CreateProjectRequest : IRequest<Result<int>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
