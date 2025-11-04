// Repository implementation for Event using EF Core: filter list, get by id   and add.
using System.Linq;  
using Microsoft.EntityFrameworkCore;
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;

namespace Sport_Calendar.Infrastructure.Repositories;

public class EfEventRepository : IEventRepository
{
    private readonly AppDbContext _db;
    public EfEventRepository(AppDbContext db) => _db = db;

    // Returns events filtered by sport and/or date range in a SINGLE DB query,
    // including related entities used by the UI (Sport, Place, Local/Visit teams).
    public async Task<List<Event>> GetFilteredAsync(int? sportId, DateOnly? from, DateOnly? to)
    {
        var q = _db.Events
            .Include(e => e.Sport)
            .Include(e => e.Place)
            .Include(e => e.LocalTeam)
            .Include(e => e.VisitTeam)
            .AsQueryable();

        if (sportId is not null) q = q.Where(e => e.SportId == sportId);
        if (from is not null) q = q.Where(e => e.EventDate >= from);
        if (to is not null) q = q.Where(e => e.EventDate <= to);

        return await q
            .OrderBy(e => e.EventDate)
            .ThenBy(e => e.EventTime)
            .ToListAsync();
    }

    // Returns one event by Id with all needed navigation properties loaded.
    public Task<Event?> GetByIdAsync(int id) =>
        _db.Events
           .Include(e => e.Sport)
           .Include(e => e.Place)
           .Include(e => e.LocalTeam)
           .Include(e => e.VisitTeam)
           .FirstOrDefaultAsync(e => e.Id == id);

    // Persists a new event (Add + SaveChangesAsync).
    public async Task AddAsync(Event e)
    {
        _db.Events.Add(e);
        await _db.SaveChangesAsync();
    }
}
