// Repository interface for Events: hides data access (EF/SQL) behind async methods used by Application services.
namespace Sport_Calendar.Application.Repositories;

using Sport_Calendar.Domain.Models;

public interface IEventRepository
{
    // Returns events filtered by sport and/or date range (inclusive).
    
    Task<List<Event>> GetFilteredAsync(int? sportId, DateOnly? from, DateOnly? to);

    // Returns a single event by its primary key
   
    Task<Event?> GetByIdAsync(int id);

    // Persists a new Event entity to the database.

    Task AddAsync(Event e);
}
