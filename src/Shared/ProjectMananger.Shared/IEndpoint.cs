using Microsoft.AspNetCore.Routing;

namespace ProjectManager.Shared;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
