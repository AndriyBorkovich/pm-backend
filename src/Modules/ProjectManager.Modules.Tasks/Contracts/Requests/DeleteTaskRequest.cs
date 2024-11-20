using Ardalis.Result;
using MediatR;

namespace ProjectManager.Modules.Tasks.Contracts.Requests;

public class DeleteTaskRequest : IRequest<Result>
{
    public int Id { get; set; }
}