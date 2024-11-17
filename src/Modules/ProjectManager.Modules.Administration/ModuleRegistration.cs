using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Modules.Administration.Features.Commands;

namespace ProjectManager.Modules.Administration;

public static class ModuleRegistration
{
    public static List<Type> All => typeof(ModuleRegistration).Assembly.GetExportedTypes()
        .Where(t => typeof(IEndpoint).IsAssignableFrom(t))
        .ToList();
    public static void AddAdministrationModule(this IServiceCollection services)
    {
        services.AddScoped<JwtHandler>();
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
    }
}
