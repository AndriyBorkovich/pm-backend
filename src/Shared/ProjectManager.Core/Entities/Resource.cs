using ProjectManager.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Core.Entities;

public sealed class Resource
{
    public int ResourceId { get; set; }
    [StringLength(100)]
    public string Name { get; set; }
    public ResourceType ResourceType { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
