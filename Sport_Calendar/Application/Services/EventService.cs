// Application service for Event use-cases; keeps controllers decoupled from data access.
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Domain.Models;

namespace Sport_Calendar.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _events;

    // Inject repository  
    public EventService(IEventRepository events) => _events = events;

    // Read: get events filtered by sport and/or date range  
    public Task<List<Event>> GetFilteredAsync(int? sportId, DateOnly? from, DateOnly? to)
        => _events.GetFilteredAsync(sportId, from, to);

    // Read: get a single event by Id 
    public Task<Event?> GetByIdAsync(int id) => _events.GetByIdAsync(id);

    // Write: create a new event 
    public Task CreateAsync(Event e) => _events.AddAsync(e);
}
