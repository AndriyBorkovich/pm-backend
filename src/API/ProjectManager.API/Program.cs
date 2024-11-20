using ProjectManager.API;
using ProjectManager.Modules.Administration;
using ProjectManager.Modules.Projects;
using ProjectManager.Modules.Tasks;
using ProjectManager.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((_, lc) => lc.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExceptionHandling();

builder.Services.AddPersistence(builder.Configuration);

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
