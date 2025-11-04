namespace Sport_Calendar.Domain.Models;
//Definition of the class Event with his attributes
public class Event
{
    public int Id { get; set; } // id_event

    public DateOnly EventDate { get; set; }
    public TimeOnly EventTime { get; set; }
    public string? Description { get; set; }

    public int SportId { get; set; }
    public Sport Sport { get; set; } = null!;

    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public int? LocalTeamId { get; set; }
    public Team? LocalTeam { get; set; }

    public int? VisitTeamId { get; set; }
    public Team? VisitTeam { get; set; }
}
