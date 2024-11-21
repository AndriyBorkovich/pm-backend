using ProjectManager.Core.Enums;
using System.ComponentModel.DataAnnotations;
using TaskStatus = ProjectManager.Core.Enums.TaskStatus;

namespace ProjectManager.Core.Entities;

public sealed class Task
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(500)]
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? Deadline { get; set; }
    public Enums.TaskStatus CurrentStatus { get; set; }
    public Priority Priority { get; set; }
    public int BoardId { get; set; }
    public Board Board { get; set; }
    public string CreatorId { get; set; }
    public User Creator { get; set; }
    public string AssigneeId { get; set; }
    public User Assignee { get; set; }
    public Dictionary<string, object> CustomFields { get; set; } = [];
    
    public static Task Create(int id, string name, string description, int boardId, string creatorId, string assigneeId)
    {
        return new Task
        {
            Id = id,
            Name = name,
            Description = description,
            CreatedDate = DateTime.UtcNow,
            CurrentStatus = TaskStatus.ToDo,
            BoardId = boardId,
            CreatorId = creatorId,
            AssigneeId = assigneeId
        };
    }

    public void UpdateStatus(TaskStatus status) => CurrentStatus = status;
    public void UpdatePriority(Priority priority) => Priority = priority;
    public void AssignToUser(User user) => Assignee = user;
    public void AddCustomField(string key, object value) => CustomFields[key] = value;
    public void RemoveCustomField(string key) => CustomFields.Remove(key);
}
