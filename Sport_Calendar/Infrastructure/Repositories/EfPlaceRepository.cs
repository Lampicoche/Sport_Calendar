// Repository implementation for Place using EF Core:  
namespace Sport_Calendar.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;

public class EfPlaceRepository : IPlaceRepository
{
    private readonly AppDbContext _db;
    public EfPlaceRepository(AppDbContext db) => _db = db;

    // Write: inserts a new Place and persists changes
    public async Task AddAsync(Place p)
    {
        _db.Places.Add(p);
        await _db.SaveChangesAsync();
    }

    // Read: returns all places without tracking for better performance on read-only queries
    public Task<List<Place>> GetAllAsync() =>
        _db.Places.AsNoTracking().ToListAsync();
}
