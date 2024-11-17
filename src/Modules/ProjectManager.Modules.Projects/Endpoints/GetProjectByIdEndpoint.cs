using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.Extensions;
using ProjectManager.Modules.Projects.Contracts.Responses;
using ProjectManager.Modules.Projects.Features.Queries;

namespace ProjectManager.Modules.Projects.Endpoints;

public sealed class GetProjectByIdEndpoint(IMediator mediator) : Endpoint<GetProjectByIdQuery, ProjectResponse>
{
    public override void Configure()
    {
        Get("/projects");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetProjectByIdQuery query, CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);
        await this.SendResponseAsync(result, r => r.Value);
    }
}
