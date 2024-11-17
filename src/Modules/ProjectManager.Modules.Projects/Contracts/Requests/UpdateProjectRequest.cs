using Ardalis.Result;
using MediatR;
using ProjectManager.Modules.Projects.Contracts.Responses;

namespace ProjectManager.Modules.Projects.Contracts.Requests;

public record UpdateProjectRequest(int Id, string Name, string Description, DateTime StartDate, DateTime EndDate) : IRequest<Result<ProjectResponse>>;
