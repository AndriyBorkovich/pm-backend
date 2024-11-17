using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Core.Entities;

public sealed class Project
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(100)]
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Board> Boards { get; set; }
    public List<Resource> Resources { get; set; }
    public List<User> Participants { get; set; }
}
