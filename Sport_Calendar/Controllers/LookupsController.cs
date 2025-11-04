// Minimal lookup API: returns small DTO lists for sports, places, and teams filtered by sport.
using Microsoft.AspNetCore.Mvc;
using Sport_Calendar.Application.Dtos;
using Sport_Calendar.Application.Services;

namespace Sport_Calendar.Controllers.Api;

 
[ApiController]
[Route("api")]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookups;

    // Inject the lookup service  
    public LookupsController(ILookupService lookups) => _lookups = lookups;

    // GET  
    [HttpGet("sports")]
    public async Task<IEnumerable<LookupItemDto>> GetSports()
        => await _lookups.GetSportsAsync();

    // GET 
    [HttpGet("places")]
    public async Task<IEnumerable<LookupItemDto>> GetPlaces()
        => await _lookups.GetPlacesAsync();

    // GET  
    [HttpGet("teams")]
    public async Task<IEnumerable<LookupItemDto>> GetTeams([FromQuery] int? sportId)
        => await _lookups.GetTeamsAsync(sportId);
}
