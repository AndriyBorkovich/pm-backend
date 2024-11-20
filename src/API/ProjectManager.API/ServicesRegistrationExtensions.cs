using Ardalis.Result;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NJsonSchema.Generation.TypeMappers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Core.Entities;
using ProjectManager.Persistence.Context;

namespace ProjectManager.API;

public static class ServicesRegistrationExtensions
{
    public static void AddExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
                ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
            }
        );
        
        services.AddExceptionHandler<ExceptionHandler>();
    }
    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole<string>>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ProjectDbContext>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
    });
    }

    public static void AddFastEndpointsFromModules(this IServiceCollection services)
    {
        services.AddFastEndpoints(o =>
        {
            // Aggregate discovered types from all modules
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("ProjectManager.Modules"));

            var allDiscoveredTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var moduleDiscoveredTypes = assembly.GetType($"{assembly.GetName().Name}.ModuleRegistration");
                if (moduleDiscoveredTypes != null)
                {
                    var typesProperty = moduleDiscoveredTypes.GetProperty("All", BindingFlags.Public | BindingFlags.Static);
                    if (typesProperty != null && typesProperty.GetValue(null) is List<Type> types)
                    {
                        allDiscoveredTypes.AddRange(types);
                    }
                }
            }

            o.SourceGeneratorDiscoveredTypes.AddRange(allDiscoveredTypes);
        }).SwaggerDocument(o => o.DocumentSettings = s =>
        {
            s.Title = "Project management API";
            s.Version = "v1";
            s.SchemaSettings.TypeMappers.Add(
            new PrimitiveTypeMapper(
                typeof(int),
                schema =>
                {
                    schema.Type = NJsonSchema.JsonObjectType.Number;
                    schema.Format = "int";
                }));
        });
    }

    public static void UseFastEndpointsWithResult(this IApplicationBuilder app)
    {
        app.UseFastEndpoints(
        c =>
        {
               c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
               c.Serializer.Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
               c.Serializer.Options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
               c.Errors.UseProblemDetails();
               c.Endpoints.Configurator =
                   ep =>
                   {
                       if (ep.ResDtoType?.IsGenericType == true && ep.ResDtoType.GetGenericTypeDefinition() == typeof(Result<>))
                       {
                           ep.Description(desc =>
                           {
                               var successType = ep.ResDtoType.GetGenericArguments()[0];
                               desc.ClearDefaultProduces()
                                   .Produces(200, successType)
                                   .ProducesProblemDetails();
                           });
                       }
                   };
           }).UseSwaggerGen();
    }
}
