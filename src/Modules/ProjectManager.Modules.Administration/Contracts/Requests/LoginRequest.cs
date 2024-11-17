using Ardalis.Result;
using MediatR;
using ProjectManager.Modules.Administration.Contracts.Responses;

namespace ProjectManager.Modules.Administration.Contracts.Requests;

public sealed class LoginRequest : IRequest<Result<LoginResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
