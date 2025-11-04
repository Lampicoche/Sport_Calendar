// Repository implementation for Sport using EF Core 
using Microsoft.EntityFrameworkCore;
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;

namespace Sport_Calendar.Infrastructure.Repositories;

public class EfSportRepository : ISportRepository
{
    private readonly AppDbContext _db;
    public EfSportRepository(AppDbContext db) => _db = db;

    // Read: returns all sports; 
    public Task<List<Sport>> GetAllAsync() =>
        _db.Sports.AsNoTracking().ToListAsync();

    // Write: inserts a new Sport and persists changes
    public async Task AddAsync(Sport s)
    {
        _db.Sports.Add(s);
        await _db.SaveChangesAsync();
    }
}
