using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManager.Modules.Projects.Features.Commands;

namespace ProjectManager.Modules.Projects.Endpoints;

public class CreateBoardValidator : AbstractValidator<CreateBoardRequest>
{
    public CreateBoardValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(200);
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}

public class CreateBoardEndpoint(IMediator mediator) : Endpoint<CreateBoardRequest, Result<int>>
{
    public override void Configure()
    {
        Post("/boards/create");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Validator<CreateBoardValidator>();
    }

    public override async Task HandleAsync(CreateBoardRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendOkAsync(result.Value, cancellation: ct);
    }
}