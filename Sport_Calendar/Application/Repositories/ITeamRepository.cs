// Repository interface for Teams: hides DB/EF details behind async methods used by Application services.
namespace Sport_Calendar.Application.Repositories;

using Sport_Calendar.Domain.Models;

public interface ITeamRepository
{
    // Returns true if a team with same name + sport +  
    // Useful to enforce a unique constraint at the service level before inserting.
    Task<bool> ExistsAsync(string name, int sportId, int? placeId);

    // Inserts a new Team and persists changes  
    Task AddAsync(Team team);

    // Retrieves all teams that belong to a given sport  
    Task<List<Team>> GetBySportAsync(int sportId);
}
