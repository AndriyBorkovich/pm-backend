﻿using Ardalis.Result;
using MediatR;
using ProjectManager.Modules.Tasks.Contracts.Responses;

namespace ProjectManager.Modules.Tasks.Contracts.Requests;

public sealed class CreateTaskRequest : IRequest<Result<TaskResponse>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public string CreatorId { get; set; }
    public string AssigneeId { get; set; }
    public int BoardId { get; set; }
}
