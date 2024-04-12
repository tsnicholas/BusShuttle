using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using App.Service;
using App.Models;
using App.Models.Driver;
using BusShuttleModel;
namespace App.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseService _database;
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    public IActionResult Index()
    {
        if(User.IsInRole("Manager"))
        {
            return RedirectToAction("Manager");
        }
        return RedirectToAction("Driver");
    }
    
    [Authorize(Roles = "Manager")]
    public IActionResult Manager()
    {
        return View();
    }

    [Authorize("IsActivated")]
    public IActionResult Driver()
    {
        List<BusShuttleModel.Loop> loops = _database.GetAllLoops();
        return View(DriverHomeModel.FromLoops(loops));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
