// Repository interface for Sports: abstracts DB access used by Application services.
using Sport_Calendar.Domain.Models;

namespace Sport_Calendar.Application.Repositories;

public interface ISportRepository
{
    // Returns all sports 
    Task<List<Sport>> GetAllAsync();

    // Inserts a new Sport and persists changes  
    Task AddAsync(Sport s);
}
