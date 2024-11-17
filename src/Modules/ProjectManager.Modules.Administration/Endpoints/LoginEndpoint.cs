using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using ProjectManager.Core.Extensions;
using ProjectManager.Modules.Administration.Contracts.Requests;
using ProjectManager.Modules.Administration.Contracts.Responses;

namespace ProjectManager.Modules.Administration.Endpoints;

public class LoginCommandValidator : AbstractValidator<LoginRequest>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public sealed class LoginEndpoint(IMediator mediator) : Endpoint<LoginRequest, Result<LoginResponse>>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
        Validator<LoginCommandValidator>();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendResponseAsync(result, r => r.Value);
    }
}
