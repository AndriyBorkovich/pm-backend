using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Core.Entities;

public sealed class Board
{
    public int Id {  get; set; }
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(200)]
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public List<Task> Tasks { get; set; }
    
    public static Board Create(int id, string name, string description, int projectId)
    {
        return new Board
        {
            Id = id,
            Name = name,
            Description = description,
            ProjectId = projectId
        };
    }

    public void AddTask(Task task) => Tasks.Add(task);
    public void RemoveTask(Task task) => Tasks.Remove(task);
}
