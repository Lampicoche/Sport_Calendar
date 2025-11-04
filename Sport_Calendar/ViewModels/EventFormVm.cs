// ViewModel used by Events/Create to bind form fields and feed dropdown lists (sports, places, teams).
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;  

namespace Sport_Calendar.ViewModels;

public class EventFormVm
{
    // Required event date  
    [Required, DataType(DataType.Date)]
    public DateOnly? EventDate { get; set; }

    // Required event time 
    [Required, DataType(DataType.Time)]
    public TimeOnly? EventTime { get; set; }

    // Optional description shown in list/details
    public string? Description { get; set; }

    // Required selections for sport and place 
    [Required] public int? SportId { get; set; }
    [Required] public int? PlaceId { get; set; }

    // Optional teams  
    public int? LocalTeamId { get; set; }
    public int? VisitTeamId { get; set; }

    // Dropdown data sources populated by the controller  
    public IEnumerable<SelectListItem> Sports { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Places { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Teams { get; set; } = Enumerable.Empty<SelectListItem>();
}
