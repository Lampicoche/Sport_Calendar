// Sports controller: display the create form, persist a new Sport using the repository, 
using Microsoft.AspNetCore.Mvc;
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Domain.Models;

namespace Sport_Calendar.Controllers;

public class SportsController : Controller
{
    private readonly ISportRepository _sports;

    // Dependency Injection 
    public SportsController(ISportRepository sports) => _sports = sports;

    // GET  
    // Renders an empty form to create a Sport
    [HttpGet]
    public IActionResult Create() => View(new Sport());

    // POST  
    // Validates model, saves through repository, and redirects to Events/Create with the new sport preselected
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Sport s)
    {
        if (!ModelState.IsValid) return View(s);

        await _sports.AddAsync(s);

         
        TempData["ok"] = "Sport created.";
        return RedirectToAction("Create", "Events", new { sportId = s.Id });

         
    }
}
