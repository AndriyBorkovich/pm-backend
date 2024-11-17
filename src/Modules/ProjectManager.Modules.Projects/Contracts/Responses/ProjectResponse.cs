namespace ProjectManager.Modules.Projects.Contracts.Responses;

public record ProjectResponse(int Id, string Name, string Description, DateTime StartDate, DateTime EndDate);
