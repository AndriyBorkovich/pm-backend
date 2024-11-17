using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using ProjectManager.Modules.Projects.Contracts.Requests;

namespace ProjectManager.Modules.Projects.Endpoints;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(100);
        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate);
    }
}

public class CreateProjectEndpoint(IMediator mediator) : Endpoint<CreateProjectRequest, Result<int>>
{
    public override void Configure()
    {
        Post("/projects/create");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Validator<CreateProjectValidator>();
    }

    public override async Task HandleAsync(CreateProjectRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);

        await SendOkAsync(result.Value, cancellation: ct);
    }
}
