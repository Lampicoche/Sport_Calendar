// Teams controller: display the create form, guard against duplicates via service, save, and continue the Events flow.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sport_Calendar.Application.Services;
using Sport_Calendar.Domain.Models;
using System.Linq;  

namespace Sport_Calendar.Controllers;

public class TeamsController : Controller
{
    private readonly ITeamService _teams;
    private readonly ILookupService _lookups;

    // Dependency Injection of application services
    public TeamsController(ITeamService teams, ILookupService lookups)
    {
        _teams = teams;
        _lookups = lookups;
    }

    // GET  
    // Renders the team creation form with sport/place dropdowns
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCombosAsync();
        return View(new Team());
    }

    // POST  
    // Validates input  and redirects to Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Team t)
    {
        if (!ModelState.IsValid)
        {
            await LoadCombosAsync();
            return View(t);
        }

         
        try
        {
            await _teams.CreateAsync(t);
        }
        catch (InvalidOperationException ex)
        {
            // Translate domain/application error into a model error for the view
            await LoadCombosAsync();
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(t);
        }

        // Continue the event-creation flow with the selected sport preselected
        return RedirectToAction("Create", "Events", new { sportId = t.SportId });
    }

    // Helper: fill dropdowns from lookup service to keep the controller thin
    private async Task LoadCombosAsync()
    {
        var sports = await _lookups.GetSportsAsync();
        var places = await _lookups.GetPlacesAsync();

        ViewBag.Sports = sports.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        ViewBag.Places = places.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
    }
}
