namespace Sport_Calendar.Domain.Models;
//Definition of the class Sport with his attributes
public class Sport
{
    public int Id { get; set; } // id_sport
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool Individual { get; set; }

    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
