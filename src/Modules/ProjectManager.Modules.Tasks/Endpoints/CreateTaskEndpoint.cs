using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManager.Core.Extensions;
using ProjectManager.Modules.Tasks.Contracts.Requests;
using ProjectManager.Modules.Tasks.Contracts.Responses;
using ProjectManager.Modules.Tasks.Features.Commands;

namespace ProjectManager.Modules.Tasks.Endpoints;

public class CreateTaskEndpoint(IMediator mediator) : Endpoint<CreateTaskRequest, Result<TaskResponse>>
{
    public override void Configure()
    {
        Post("/tasks/create");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Validator<CreateTaskValidator>();
    }

    public override async Task HandleAsync(CreateTaskRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);

        await this.SendResponseAsync(result, r => r.Value);
    }
}