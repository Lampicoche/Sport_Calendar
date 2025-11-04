// Application service contract for lookup data 
namespace Sport_Calendar.Application.Services;

using Sport_Calendar.Application.Dtos;

public interface ILookupService
{
    // Returns a list of sports as (Id, Name) items.
    Task<List<LookupItemDto>> GetSportsAsync();

    // Returns a list of places as (Id, Name) items.
    Task<List<LookupItemDto>> GetPlacesAsync();

    // Returns teams for the given sport as (Id, Name) items.
    // Implementations typically return an empty list when sportId is null.
    Task<List<LookupItemDto>> GetTeamsAsync(int? sportId);
}
