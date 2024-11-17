using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManager.Core.Extensions;
using ProjectManager.Modules.Projects.Contracts.Requests;
using ProjectManager.Modules.Projects.Contracts.Responses;

namespace ProjectManager.Modules.Projects.Endpoints;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(100);
        RuleFor(x => x.StartDate)
           .GreaterThanOrEqualTo(DateTime.Now)
           .WithMessage("Start date must be in future")
           .LessThanOrEqualTo(x => x.EndDate);
    }
}

public sealed class UpdateProjectEndpoint(IMediator mediator) : Endpoint<UpdateProjectRequest, ProjectResponse>
{
    public override void Configure()
    {
        Put("/projects/update");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Validator<UpdateProjectValidator>();
    }

    public override async Task HandleAsync(UpdateProjectRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendResponseAsync(result, r => r.Value);
    }
}
