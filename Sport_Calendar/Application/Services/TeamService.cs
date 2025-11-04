// Application service for Team use-cases; 
namespace Sport_Calendar.Application.Services;

using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Domain.Models;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teams;

    // Inject repository  
    public TeamService(ITeamRepository teams) => _teams = teams;

    // Write: create a new team ensuring no duplicates (Name + SportId + optional PlaceId)
    public async Task CreateAsync(Team t)
    {
        var exists = await _teams.ExistsAsync(t.Name, t.SportId, t.PlaceId);
        if (exists) throw new InvalidOperationException("This team already exists for that sport/place.");
        await _teams.AddAsync(t);
    }

    // Read: list all teams belonging to a given sport
    public Task<List<Team>> GetBySportAsync(int sportId) => _teams.GetBySportAsync(sportId);
}
