// Repository interface for Places: abstracts data access  
namespace Sport_Calendar.Application.Repositories;

using Sport_Calendar.Domain.Models;

public interface IPlaceRepository
{
    // Adds a new Place entity and persists it
    Task AddAsync(Place p);

    // Retrieves all places 
    Task<List<Place>> GetAllAsync();
}
