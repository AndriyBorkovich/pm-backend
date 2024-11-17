using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Modules.Projects;

public static class ModuleRegistration
{
    public static List<Type> All => typeof(ModuleRegistration).Assembly.GetExportedTypes()
       .Where(t => typeof(IEndpoint).IsAssignableFrom(t))
       .ToList();
    public static void AddProjectsModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
    }
}
