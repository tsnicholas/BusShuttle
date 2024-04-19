using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.DriverModels;
using App.Service;
using Database.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize("IsActivated")]
public class DriverController : Controller
{
    private readonly ILogger<DriverController> _logger;
    private readonly IDatabaseService _database;
    private readonly IAccountService _accountService;

    public DriverController(ILogger<DriverController> logger, IAccountService accountService, IDatabaseService database)
    {
        _logger = logger;
        _accountService = accountService;
        _database = database;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Loop> loops = _database.GetAll<Loop>();
        List<Bus> buses = _database.GetAll<Bus>();
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
        Bus bus = _database.GetById<Bus>(busId);
        Loop loop = _database.GetLoopWithStopsById(loopId);
        List<Stop> stops = GenerateStopList(loop);
        return View(LoopEntryModel.CreateModel(driver, bus, loop, stops));
    }

    private List<Stop> GenerateStopList(Loop loop)
    {
        List<Stop> output = new List<Stop>();
        foreach(var route in loop.Routes)
        {
            output.Add(route.Stop ?? throw new InvalidOperationException());
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
            int nextId = _database.GetAll<Entry>().Count() + 1;
            Entry newEntry = new Entry(nextId, model.Boarded, model.LeftBehind);
            newEntry.SetBus(_database.GetById<Bus>(model.BusId));
            newEntry.SetDriver(_database.GetById<Driver>(model.DriverId));
            newEntry.SetLoop(_database.GetById<Loop>(model.LoopId));
            newEntry.SetStop(_database.GetById<Stop>(model.StopId));
            _database.CreateEntity<Entry>(newEntry);
        });
        return View(model);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
