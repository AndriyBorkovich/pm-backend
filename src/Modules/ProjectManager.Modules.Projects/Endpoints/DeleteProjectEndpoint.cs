using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManager.Core.Extensions;
using ProjectManager.Modules.Projects.Contracts.Requests;

namespace ProjectManager.Modules.Projects.Endpoints;

public sealed class DeleteProjectEndpoint(IMediator mediator) : Endpoint<DeleteProjectRequest, Result<string>>
{
    public override void Configure()
    {
        Delete("/projects/delete");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(DeleteProjectRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);

        await this.SendResponseAsync(result, r => r.Value);
    }
}
