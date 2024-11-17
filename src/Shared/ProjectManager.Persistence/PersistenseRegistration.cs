using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Persistence.Context;

namespace ProjectManager.Persistence;

public static class PersistenseRegistration
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProjectDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Default"));
        });
    }
}
