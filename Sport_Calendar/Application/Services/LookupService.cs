// Application service that returns small DTO lists  
using System.Linq; 
using Sport_Calendar.Application.Dtos;
using Sport_Calendar.Application.Repositories;

namespace Sport_Calendar.Application.Services;

public class LookupService : ILookupService
{
    private readonly ISportRepository _sports;
    private readonly IPlaceRepository _places;
    private readonly ITeamRepository _teams;

    // Inject repositories  
    public LookupService(ISportRepository sports, IPlaceRepository places, ITeamRepository teams)
        => (_sports, _places, _teams) = (sports, places, teams);

    // Returns sports  
    public async Task<List<LookupItemDto>> GetSportsAsync()
        => (await _sports.GetAllAsync())
            .OrderBy(s => s.Name)
            .Select(s => new LookupItemDto(s.Id, s.Name))
            .ToList();

    // Returns places  
    public async Task<List<LookupItemDto>> GetPlacesAsync()
        => (await _places.GetAllAsync())
            .OrderBy(p => p.Name)
            .Select(p => new LookupItemDto(p.Id, p.Name))
            .ToList();

    // Returns teams (Id, Name) for a given sport; empty list if no sport selected
    public async Task<List<LookupItemDto>> GetTeamsAsync(int? sportId)
    {
        if (sportId is null) return new(); // UI can show "(none)" when empty
        var teams = await _teams.GetBySportAsync(sportId.Value);
        return teams
            .OrderBy(t => t.Name)
            .Select(t => new LookupItemDto(t.Id, t.Name))
            .ToList();
    }
}
