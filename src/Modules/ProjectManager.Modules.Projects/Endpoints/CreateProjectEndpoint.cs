using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using ProjectManager.Modules.Projects.Contracts.Requests;

namespace ProjectManager.Modules.Projects.Endpoints;

public class CreateProjectEndpoint(IMediator mediator) : Endpoint<CreateProjectRequest, int>
{
    public override void Configure()
    {
        Post("/projects/create");
        Description(b => b.Accepts<CreateProjectRequest>()
                          .Produces<int>(StatusCodes.Status200OK));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CreateProjectRequest req, CancellationToken ct)
    {
        var projectId = await mediator.Send(req, ct);

        await SendOkAsync(projectId, cancellation: ct);
    }
}
