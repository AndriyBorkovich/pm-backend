using Microsoft.AspNetCore.Identity;
using ProjectManager.Core.Enums;

namespace ProjectManager.Core.Entities;

public sealed class User : IdentityUser
{
    public NotificationType SendNotificationType { get; set; }
    public List<Project> Projects { get; set; }
    public List<Task> CreatedTasks { get; set; }
    public List<Task> AssignedTasks { get; set; }
}
