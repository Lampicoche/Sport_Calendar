// Home endpoints: redirect "/" or /Home/Index to Events/Index; keep Privacy and Error actions.
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sport_Calendar.Domain.Models;

namespace Sport_Calendar.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // DI for logging 
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Redirect Home/Index to Events/Index to avoid requiring Views/Home/Index.cshtml
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Events");
    }

    
    public IActionResult Privacy()
    {
        return View();
         
    }

    // Standard error endpoint 
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
