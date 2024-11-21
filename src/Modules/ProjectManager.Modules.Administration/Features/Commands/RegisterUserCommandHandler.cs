using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Core.Entities;
using ProjectManager.Core.Enums;
using ProjectManager.Modules.Administration.Contracts.Requests;
using ProjectManager.Modules.Administration.Contracts.Responses;

namespace ProjectManager.Modules.Administration.Features.Commands;

public sealed class RegisterUserCommandHandler(UserManager<User> userManager) : IRequestHandler<RegisterUserRequest, Result<RegistrationResponse>>
{
    public async Task<Result<RegistrationResponse>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var userName = $"{request.FirstName}{request.LastName}";
        var user = User.Create(request.Email, userName, NotificationType.Push);

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            return new RegistrationResponse
            {
                IsSuccessfulRegistration = false,
                Errors = errors
            };
        }

        await userManager.AddToRoleAsync(user, request.Role);

        return new RegistrationResponse
        {
            IsSuccessfulRegistration = true
        };
    }
}
