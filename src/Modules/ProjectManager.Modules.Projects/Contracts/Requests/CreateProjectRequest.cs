using MediatR;

namespace ProjectManager.Modules.Projects.Contracts.Requests;

public class CreateProjectRequest : IRequest<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
