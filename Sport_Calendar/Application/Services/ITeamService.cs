// Application service contract for Teams 
namespace Sport_Calendar.Application.Services;

using Sport_Calendar.Domain.Models;

public interface ITeamService
{
    // Creates a new team.  
    
    Task CreateAsync(Team t);

    // Returns all teams that belong to the specified sport 
    Task<List<Team>> GetBySportAsync(int sportId);
}
