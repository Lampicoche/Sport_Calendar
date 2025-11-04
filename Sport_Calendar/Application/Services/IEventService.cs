// Application service contract for Events: controllers depend on this  
namespace Sport_Calendar.Application.Services;

using Sport_Calendar.Domain.Models;

public interface IEventService
{
    // Read: returns events filtered by sport and/or inclusive date range.
    Task<List<Event>> GetFilteredAsync(int? sportId, DateOnly? from, DateOnly? to);

    // Read: returns a single event by Id  
    Task<Event?> GetByIdAsync(int id);

    // Write: creates/persists a new event.
    Task CreateAsync(Event e);
}
