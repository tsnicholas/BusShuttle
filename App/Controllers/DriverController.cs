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
        var loop = _database.GetLoopWithStopsById(loopId);
        List<Stop> stops = GenerateStopList(loop);
        return View(LoopEntryModel.CreateModel(driver, bus, loop, stops));
    }

    private List<Stop> GenerateStopList(Loop loop)
    {
        List<Stop> output = new List<Stop>();
        foreach(var route in loop.Routes)
        {
            output.Add(route.Stop);
        }
        return output;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitEntry(LoopEntryModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        var routeParams = new RouteValueDictionary();
        await Task.Run(() => {
            Stop selectedStop = _database.GetStopById(model.StopId);
            int nextId = _database.GetAllEntries().Count() + 1;
            Entry newEntry = new Entry(nextId, model.Boarded, model.LeftBehind, model.TheBus, 
                model.BusDriver, model.BusLoop, selectedStop);
            _database.CreateEntry(newEntry);
             routeParams.Add("busId", model.TheBus.Id);
            routeParams.Add("loopId", model.BusLoop.Id);
        });
        return RedirectToAction("Index", routeParams);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
