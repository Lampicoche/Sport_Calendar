// Repository implementation for Team using EF Core 
namespace Sport_Calendar.Infrastructure.Repositories;

using System.Linq; 
using Microsoft.EntityFrameworkCore;
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;

public class EfTeamRepository : ITeamRepository
{
    private readonly AppDbContext _db;
    public EfTeamRepository(AppDbContext db) => _db = db;

    // Read: returns true if a team with same Name + SportId  
    public Task<bool> ExistsAsync(string name, int sportId, int? placeId)
        => _db.Teams.AnyAsync(t => t.Name == name && t.SportId == sportId && t.PlaceId == placeId);

    // Write: inserts a new team and persists changes
    public async Task AddAsync(Team team)
    {
        _db.Teams.Add(team);
        await _db.SaveChangesAsync();
    }

    // Read: lists teams for a given sport  
    public Task<List<Team>> GetBySportAsync(int sportId)
        => _db.Teams
              .Where(t => t.SportId == sportId)
              .OrderBy(t => t.Name)
              .AsNoTracking()
              .ToListAsync();
}
