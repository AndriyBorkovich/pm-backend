using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
namespace ProjectManager.Modules.Tasks;

public static class ModuleRegistration
{
    public static List<Type> All => typeof(ModuleRegistration).Assembly.GetExportedTypes()
        .Where(t => typeof(IEndpoint).IsAssignableFrom(t))
        .ToList();
    public static void AddTasksModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
    }
}
