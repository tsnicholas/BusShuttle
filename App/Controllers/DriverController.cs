using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Driver;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

public class DriverController : Controller
{
    private readonly ILogger<DriverController> _logger;
    private readonly DatabaseService _database;

    public DriverController(ILogger<DriverController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    public IActionResult Index([FromRoute] int loopId)
    {
        var loop = _database.GetLoopWithId(loopId);

        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
