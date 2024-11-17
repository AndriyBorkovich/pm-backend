using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using ProjectManager.Core.Constants;
using ProjectManager.Modules.Administration.Contracts.Requests;
using ProjectManager.Modules.Administration.Contracts.Responses;

namespace ProjectManager.Modules.Administration.Endpoints;

internal class RegisterCommandValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Role).Must(x => x.Equals(Roles.Admin) || x.Equals(Roles.Manager) || x.Equals(Roles.Developer) || x.Equals(Roles.Developer));
    }
}

public sealed class RegisterEndpoint(IMediator mediator) : Endpoint<RegisterUserRequest, Result<RegistrationResponse>>
{
    public override void Configure()
    {
        Post("/auth/register");
        AllowAnonymous();
        Validator<RegisterCommandValidator>();
    }

    public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendResponseAsync(result, r => r.Value);
    }
}