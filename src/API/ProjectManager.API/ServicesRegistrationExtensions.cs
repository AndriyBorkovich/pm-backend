using Ardalis.Result;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace ProjectManager.API;

public static class ServicesRegistrationExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Project management API", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
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
        });
    }

    public static void UseFastEndpointsWithResult(this IApplicationBuilder app)
    {
        app.UseFastEndpoints(
           c =>
           {
               c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
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
           });
    }
}
