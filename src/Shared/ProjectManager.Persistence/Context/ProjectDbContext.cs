using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Core.Entities;
using ProjectManager.Persistence.Converters;
using TaskEntity = ProjectManager.Core.Entities.Task;

namespace ProjectManager.Persistence.Context;

public class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : IdentityDbContext<User, IdentityRole<string>, string>(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<TaskHistory> TasksHistory { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Project>()
            .HasMany(p => p.Participants)
            .WithMany(u => u.Projects)
            .UsingEntity("ProjectAssignments");

        builder.Entity<Resource>()
            .Property(r => r.ResourceType)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Entity<TaskEntity>()
            .Property(t => t.CustomFields)
            .HasConversion(new CustomFieldsConverter())
            .HasColumnType("jsonb");

        builder.Entity<TaskEntity>()
            .Property(t => t.Priority)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Entity<TaskEntity>()
            .Property(t => t.CurrentStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Entity<TaskEntity>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks);

        builder.Entity<TaskEntity>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.CreatedTasks);

        builder.Entity<Notification>()
            .Property(n => n.Message)
            .HasColumnType("jsonb");

        builder.Entity<TaskHistory>()
            .Property(t => t.PayloadJson)
            .HasColumnType("jsonb");

        base.OnModelCreating(builder);
    }
}
