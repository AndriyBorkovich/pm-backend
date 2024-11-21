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
    
    public static Resource Create(int resourceId, string name, ResourceType resourceType, int projectId)
    {
        return new Resource
        {
            ResourceId = resourceId,
            Name = name,
            ResourceType = resourceType,
            ProjectId = projectId
        };
    }

    public void UpdateResourceDetails(string name, ResourceType resourceType)
    {
        Name = name;
        ResourceType = resourceType;
    }
}
