// ViewModel used by Events/Index to capture filters and feed the sports dropdown.
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sport_Calendar.ViewModels;

public class EventFilterVm
{
    // Selected sport to filter 
    public int? SportId { get; set; }

    // Start date filter 
    public DateOnly? From { get; set; }

    // End date filter  
    public DateOnly? To { get; set; }

    // Dropdown items for the Sport  
    public IEnumerable<SelectListItem> Sports { get; set; } = Enumerable.Empty<SelectListItem>();
}
