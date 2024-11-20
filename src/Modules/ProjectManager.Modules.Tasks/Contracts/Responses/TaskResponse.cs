using ProjectManager.Core.Enums;
using TaskStatus = ProjectManager.Core.Enums.TaskStatus;

namespace ProjectManager.Modules.Tasks.Contracts.Responses;

public class TaskResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? Deadline { get; set; }
    public TaskStatus CurrentStatus { get; set; }
    public Priority Priority { get; set; }
    public string CreatorId { get; set; }
    public string AssigneeId { get; set; }
    public int BoardId { get; set; }
}