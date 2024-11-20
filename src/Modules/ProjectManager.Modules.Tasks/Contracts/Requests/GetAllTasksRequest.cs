using Ardalis.Result;
using MediatR;
using ProjectManager.Core.Enums;
using ProjectManager.Modules.Tasks.Contracts.Responses;
using TaskStatus = ProjectManager.Core.Enums.TaskStatus;

namespace ProjectManager.Modules.Tasks.Contracts.Requests;

public class GetAllTasksRequest : IRequest<Result<List<TaskResponse>>>
{
    public string AssigneeId { get; set; }
    public TaskStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
}