using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Core.Entities;

public sealed class Notification
{
    public int Id { get; set; }
    [StringLength(100), Required]
    public string MessageType { get; set; }
    [StringLength(1000), Required]
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}
