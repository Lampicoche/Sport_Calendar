// Places controller: display create form and persist a new Place using the repository abstraction.
using Microsoft.AspNetCore.Mvc;
using Sport_Calendar.Application.Repositories;  
using Sport_Calendar.Domain.Models;

namespace Sport_Calendar.Controllers;

public class PlacesController : Controller
{
    private readonly IPlaceRepository _places;

    // Dependency Injection 
    public PlacesController(IPlaceRepository places) => _places = places;

    // GET  
    // Renders an empty form to create a Place
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Place());
    }

    // POST  
    // Validates model, saves through repository, and redirects to Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Place p)
    {
        // Server-side validation (data annotations, etc.)
        if (!ModelState.IsValid) return View(p);

        // Persist via repository (no direct DbContext usage here)
        await _places.AddAsync(p);

        // Continue the event-creation flow after adding a place
        return RedirectToAction("Create", "Events");
    }
}
