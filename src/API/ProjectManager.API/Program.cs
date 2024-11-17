using Microsoft.AspNetCore.Identity;
using ProjectManager.API;
using ProjectManager.Core.Entities;
using ProjectManager.Modules.Administration;
using ProjectManager.Modules.Projects;
using ProjectManager.Modules.Tasks;
using ProjectManager.Persistence;
using ProjectManager.Persistence.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((_, lc) => lc.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwagger();

builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
        ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
    }
);
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddIdentity<User, IdentityRole<string>>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;
    opt.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ProjectDbContext>();

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddAdministrationModule();
builder.Services.AddProjectsModule();
builder.Services.AddTasksModule();

builder.Services.AddFastEndpointsFromModules();

var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.UseFastEndpointsWithResult();

app.Run();
