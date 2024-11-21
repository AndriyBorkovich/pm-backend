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
    
    public static Project Create(int id, string name, string description, DateTime startDate, DateTime endDate)
    {
        return new Project
        {
            Id = id,
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public void AddBoard(Board board) => Boards.Add(board);
    public void RemoveBoard(Board board) => Boards.Remove(board);
    public void AddResource(Resource resource) => Resources.Add(resource);
    public void RemoveResource(Resource resource) => Resources.Remove(resource);
    public void AddParticipant(User user) => Participants.Add(user);
    public void RemoveParticipant(User user) => Participants.Remove(user);
}
