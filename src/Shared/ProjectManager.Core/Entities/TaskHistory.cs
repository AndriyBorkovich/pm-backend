using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Core.Entities;

public sealed class TaskHistory
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
    [StringLength(100)]
    public string PayloadClassName { get; set; }
    [StringLength(1000)]
    public string PayloadJson { get; set; }
    public DateTime Timestamp { get; set; }
}
