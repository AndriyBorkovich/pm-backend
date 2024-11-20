using Ardalis.Result;
using MediatR;
using ProjectManager.Core.Enums;
using ProjectManager.Modules.Tasks.Contracts.Responses;
using TaskStatus = ProjectManager.Core.Enums.TaskStatus;

namespace ProjectManager.Modules.Tasks.Contracts.Requests;

public sealed class UpdateTaskRequest : IRequest<Result<TaskResponse>>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public TaskStatus CurrentStatus { get; set; }
    public Priority Priority { get; set; }
    public string AssigneeId { get; set; }
}