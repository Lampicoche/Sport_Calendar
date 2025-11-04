// DTOs used to transfer data between the Application layer and Web/API without exposing EF entities.
namespace Sport_Calendar.Application.Dtos;

public record LookupItemDto(int Id, string Name);

// --- Full Data Transfer Objects ---
public record SportDto(
    int Id,
    string Name,
    string? Description,
    bool Individual
);

public record PlaceDto(
    int Id,
    string Name,
    string? City,
    int? Capacity,
    decimal? TicketPrice,
    bool Vip
);

public record TeamDto(
    int Id,
    string Name,
    int SportId,
    int? PlaceId
);

// DTO used when creating an Event (date/time as strings for HTML/JSON friendliness)
public record EventCreateDto(
    string EventDate,   // yyyy-MM-dd
    string EventTime,   // HH:mm
    string? Description,
    int SportId,
    int PlaceId,
    int? LocalTeamId,
    int? VisitTeamId
);

// Compact item used in event listings
public record EventListItemDto(
    int Id,
    string Date,
    string Time,
    string? Description,
    SportDto Sport,     // full object
    PlaceDto Place,     // full object
    TeamDto? LocalTeam, // full object (nullable)
    TeamDto? VisitTeam  // full object (nullable)
);

// Full detail for a single event
public record EventDetailDto(
    int Id,
    string Date,
    string Time,
    string? Description,
    SportDto Sport,
    PlaceDto Place,
    TeamDto? LocalTeam,
    TeamDto? VisitTeam
);
