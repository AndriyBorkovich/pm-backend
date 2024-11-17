using Ardalis.Result;
using MediatR;
using ProjectManager.Modules.Administration.Contracts.Responses;

namespace ProjectManager.Modules.Administration.Contracts.Requests;

public sealed class RegisterUserRequest : IRequest<Result<RegistrationResponse>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
