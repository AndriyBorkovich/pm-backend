using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Infrastructure.Services;

namespace ProjectManager.Infrastructure;

public static class InfrastructureRegistration
{
    public static void AddInfra(this IServiceCollection services)
    {
        services.AddSingleton<IFileStorageService, S3FileStorageService>();
    }
}