using Microsoft.EntityFrameworkCore;
using ProjectManager.Persistence.Context;
using SystemTask = System.Threading.Tasks.Task;

namespace ProjectManager.UnitTests;

public class TestDbContextFixture : IAsyncLifetime
{
    public ProjectDbContext DbContext { get; private set; }

    public TestDbContextFixture()
    {
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerTestDb")
            .Options;

        DbContext = new ProjectDbContext(options);
    }

    public SystemTask InitializeAsync() => SystemTask.CompletedTask;

    public async SystemTask DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.DisposeAsync();
    }
}