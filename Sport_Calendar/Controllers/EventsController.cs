// MVC controller for Events: list/filter, create, details  , and "SeeEvent" shortcut.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sport_Calendar.Application.Services;
using Sport_Calendar.ViewModels;
using Sport_Calendar.Domain.Models;
using System.Linq;

namespace Sport_Calendar.Controllers;

public class EventsController : Controller
{
    private readonly IEventService _events;
    private readonly ILookupService _lookups;

    public EventsController(IEventService events, ILookupService lookups)
        => (_events, _lookups) = (events, lookups);

    // GET /Events
    // Lists events using optional filters  
     
    public async Task<IActionResult> Index(EventFilterVm filter)
    {
        var list = await _events.GetFilteredAsync(filter.SportId, filter.From, filter.To);
        ViewBag.Events = list;

        var sports = await _lookups.GetSportsAsync();
        filter.Sports = sports.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

        return View(filter); // Renders Views/Events/Index.cshtml
    }

    // GET  
    // Shows the create form; preloads dropdowns and optionally preselects the given sport.
    [HttpGet]
    public async Task<IActionResult> Create(int? sportId)
    {
        var vm = new EventFormVm { SportId = sportId };
        await LoadCombosAsync(vm, sportId);
        return View(vm); // Renders Views/Events/Create.cshtml
    }

    // POST  
    // Validates input, prevents same team on both sides, creates the event, and redirects to Index.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventFormVm vm)
    {
        if (!ModelState.IsValid)
        {
            await LoadCombosAsync(vm, vm.SportId);
            return View(vm);
        }

        if (vm.LocalTeamId.HasValue && vm.VisitTeamId.HasValue && vm.LocalTeamId == vm.VisitTeamId)
        {
            ModelState.AddModelError(string.Empty, "Local and visiting teams cannot be the same.");
            await LoadCombosAsync(vm, vm.SportId);
            return View(vm);
        }

        var e = new Event
        {
            EventDate = vm.EventDate!.Value,
            EventTime = vm.EventTime!.Value,
            Description = vm.Description,
            SportId = vm.SportId!.Value,
            PlaceId = vm.PlaceId!.Value,
            LocalTeamId = vm.LocalTeamId,
            VisitTeamId = vm.VisitTeamId
        };

        await _events.CreateAsync(e);
        TempData["ok"] = "Event created.";
        return RedirectToAction(nameof(Index));
    }

    // GET / 
    // Shows a single event and computes Prev/Next IDs to navigate with arrows.
    public async Task<IActionResult> Details(int id)
    {
        var e = await _events.GetByIdAsync(id);
        if (e is null) return NotFound();

        // Stable order for navigation: by date, then time, then Id
        var all = await _events.GetFilteredAsync(null, null, null);
        var ordered = all
            .OrderBy(x => x.EventDate)
            .ThenBy(x => x.EventTime)
            .ThenBy(x => x.Id)
            .ToList();

        var count = ordered.Count;
        var idx = ordered.FindIndex(x => x.Id == id);

        if (count > 0 && idx >= 0)
        {
            var prevId = ordered[(idx - 1 + count) % count].Id; // wrap to last if at first
            var nextId = ordered[(idx + 1) % count].Id;         // wrap to first if at last
            ViewBag.PrevId = prevId;
            ViewBag.NextId = nextId;
        }

        // Useful for navbar links that need the current event id
        ViewData["CurrentEventId"] = id;

        return View(e); // Renders Views/Events/Details.cshtml
    }

    // GET  
    // Redirects to Details of a specific id   or to the latest event; if none exists, goes to Create.
    [HttpGet]
    public async Task<IActionResult> SeeEvent(int? id)
    {
        if (id.HasValue)
            return RedirectToAction("Details", new { id = id.Value });

        // Pick latest by date/time; fallback to highest Id
        var all = await _events.GetFilteredAsync(null, null, null);
        var ev = all
            .OrderByDescending(e => e.EventDate)
            .ThenByDescending(e => e.EventTime)
            .FirstOrDefault()
            ?? all.OrderByDescending(e => e.Id).FirstOrDefault();

        if (ev is null)
        {
            TempData["ok"] = "No events yet. Please create one.";
            return RedirectToAction("Create");
        }

        return RedirectToAction("Details", new { id = ev.Id });
    }

    // Helper: fills dropdowns (sports, places, teams filtered by sport)
    private async Task LoadCombosAsync(EventFormVm vm, int? sportId)
    {
        var sports = await _lookups.GetSportsAsync();
        var places = await _lookups.GetPlacesAsync();
        var teams = await _lookups.GetTeamsAsync(sportId);

        vm.Sports = sports.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        vm.Places = places.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        vm.Teams = teams.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
    }
}
