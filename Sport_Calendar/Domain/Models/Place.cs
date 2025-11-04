//Definition of the class Place with his attributes

//Definition of the class Place with his attributes
namespace Sport_Calendar.Domain.Models;

public class Place
{
    public int Id { get; set; }           // id_place
    public string Name { get; set; } = null!; // place
    public string? City { get; set; }
    public int? Capacity { get; set; }
    public decimal? TicketPrice { get; set; }
    public bool Vip { get; set; }

    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}

