namespace ProjectManager.Modules.Administration.Contracts.Responses;

public sealed class RegistrationResponse
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string> Errors { get; set; }
}
