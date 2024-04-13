using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Driver;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize("IsActivated")]
public class DriverController : Controller
{
    private readonly ILogger<DriverController> _logger;
    private readonly DatabaseService _database;
    private readonly IAccountService _accountService;

    public DriverController(ILogger<DriverController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
        _database = new DatabaseService();
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int busId, [FromQuery] int loopId)
    {
        string email = await _accountService.GetCurrentEmail(HttpContext.User);
        var driver = _database.GetDriverByEmail(email);
        var bus = _database.GetBusById(busId);
        var loop = _database.GetLoopWithId(loopId);
        List<Stop> stops = GenerateStopList(loop);
        return View(LoopEntryModel.CreateModel(driver, bus, loop, stops));
    }

    private List<Stop> GenerateStopList(Loop loop)
    {
        List<Stop> allStops = _database.GetAllStops();
        List<Stop> loopStops = new List<Stop>();
        foreach(var route in loop.Routes)
        {
            var stop = allStops.Single(stop => stop.RouteId == route.Id);
            if(stop == null) continue;
            loopStops.Add(stop);
        }
        return loopStops;
    }

    [HttpGet]
    public IActionResult SignInToLoop()
    {
        List<Loop> loops = _database.GetAllLoops();
        List<Bus> buses = _database.GetAllBuses();
        return View(DriverHomeModel.CreateUsingLists(loops, buses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignInToLoop(DriverHomeModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        var routeDictionary = new RouteValueDictionary();
        await Task.Run(() => {
            routeDictionary.Add("busId", model.BusId);
            routeDictionary.Add("loopId", model.LoopId);
        });
        return RedirectToAction("Index", routeDictionary);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
