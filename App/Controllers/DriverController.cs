using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.DriverModels;
using App.Service;
using Database.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize("IsActivated")]
public class DriverController(IAccountService accountService, IDatabaseService database) : Controller
{
    private static readonly string BUS_ID_KEY = "busId";
    private static readonly string LOOP_ID_KEY = "loopId";
    private readonly IDatabaseService _database = database;
    private readonly IAccountService _accountService = accountService;

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
        RouteValueDictionary routeDictionary = [];
        await Task.Run(() => {
            routeDictionary.Add(BUS_ID_KEY, model.BusId);
            routeDictionary.Add(LOOP_ID_KEY, model.LoopId);
        });
        return RedirectToAction("EntryForm", routeDictionary);
    }

    [HttpGet]
    public async Task<IActionResult> EntryForm([FromQuery] int busId, [FromQuery] int loopId)
    {
        string email = await _accountService.GetCurrentEmail(ControllerContext.HttpContext);
        int nextId = _database.GenerateId<Entry>();
        Driver driver = _database.GetDriverByEmail(email);
        Bus? bus = _database.GetById<Bus>(busId);
        Loop? loop = _database.GetLoopWithStopsById(loopId);
        if(bus == null || loop == null)
        {
            return RedirectToAction("Index");
        }
        List<Stop> stops = GenerateStopList(loop);
        return View(LoopEntryModel.CreateModel(nextId, driver, bus, loop, stops));
    }

    private static List<Stop> GenerateStopList(Loop loop)
    {
        List<Stop> output = [];
        foreach(var route in loop.Routes)
        {
            if(route.Stop == null) 
            {
                continue;
            }
            output.Add(route.Stop);
        }
        return output;
    }

    [HttpPost]
    public async Task<IActionResult> EntryForm([Bind("DriverId,BusId,LoopId,StopId,Boarded,LeftBehind")]LoopEntryModel model)
    {
        if(ModelState.IsValid)
        {
            await Task.Run(() => {
                _database.CreateEntity(new Entry(model.Id, model.Boarded, model.LeftBehind)
                    .SetBus(_database.GetById<Bus>(model.BusId) ?? throw new InvalidOperationException())
                    .SetDriver(_database.GetById<Driver>(model.DriverId) ?? throw new InvalidOperationException())
                    .SetLoop(_database.GetById<Loop>(model.LoopId) ?? throw new InvalidOperationException())
                    .SetStop(_database.GetById<Stop>(model.StopId) ?? throw new InvalidOperationException()));
            });
        }
        RouteValueDictionary routeDictionary = [];
        routeDictionary.Add(BUS_ID_KEY, model.BusId);
        routeDictionary.Add(LOOP_ID_KEY, model.LoopId);
        return RedirectToAction("EntryForm", routeDictionary);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
