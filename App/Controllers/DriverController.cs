using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.DriverModels;
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
    public IActionResult Index()
    {
        List<Loop> loops = _database.GetAllLoops();
        List<Bus> buses = _database.GetAllBuses();
        return View(DriverHomeModel.CreateUsingLists(loops, buses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DriverHomeModel model)
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
        return RedirectToAction("EntryForm", routeDictionary);
    }

    [HttpGet]
    public async Task<IActionResult> EntryForm([FromQuery] int busId, [FromQuery] int loopId)
    {
        string email = await _accountService.GetCurrentEmail(HttpContext.User);
        Driver driver = _database.GetDriverByEmail(email);
        Bus bus = _database.GetBusById(busId);
        Loop loop = _database.GetLoopWithStopsById(loopId);
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

    [HttpPost]
    public async Task<IActionResult> EntryForm([Bind("DriverId,BusId,LoopId,StopId,Boarded,LeftBehind")]LoopEntryModel model)
    {
        var routeParams = new RouteValueDictionary();
        await Task.Run(() => {
            routeParams.Add("busId", model.BusId);
            routeParams.Add("loopId", model.LoopId);
        });
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        await Task.Run(() => {
            Driver driver = _database.GetDriverById(model.DriverId);
            Bus bus = _database.GetBusById(model.BusId);
            Loop loop = _database.GetLoopWithId(model.LoopId);
            Stop selectedStop = _database.GetStopById(model.StopId);
            int nextId = _database.GetAllEntries().Count() + 1;
            Entry newEntry = new Entry(nextId, model.Boarded, model.LeftBehind, bus, driver, loop, selectedStop);
            bus.AddEntry(newEntry);
            driver.AddEntry(newEntry);
            loop.AddEntry(newEntry);
            selectedStop.AddEntry(newEntry);
            _database.CreateEntry(newEntry);
        });
        return View(model);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
