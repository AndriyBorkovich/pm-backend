using Microsoft.AspNetCore.Identity;
using ProjectManager.Core.Enums;

namespace ProjectManager.Core.Entities;

public sealed class User : IdentityUser
{
    public NotificationType SendNotificationType { get; set; }
    public List<Project> Projects { get; set; }
    public List<Task> CreatedTasks { get; set; }
    public List<Task> AssignedTasks { get; set; }

    public User() { }

    public static User Create(string email, string userName, NotificationType sendNotificationType)
    {
        return new User
        {
            Email = email,
            NormalizedEmail = email.ToUpper(),
            UserName = userName,
            NormalizedUserName = userName.ToUpper(),
            SendNotificationType = sendNotificationType
        };
    }

    public void CreateTask(Task task) => CreatedTasks.Add(task);
    public void AssignTask(Task task) => AssignedTasks.Add(task);
    public void AddProject(Project project) => Projects.Add(project);
    public void RemoveProject(Project project) => Projects.Remove(project);
}
