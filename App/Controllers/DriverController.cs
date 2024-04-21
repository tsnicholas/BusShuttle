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
        Driver driver = _database.GetDriverByEmail(email);
        Bus bus = _database.GetById<Bus>(busId);
        Loop loop = _database.GetById<Loop>(loopId, "BusRoute");
        List<Stop> stops = GenerateStopList(loop);
        return View(LoopEntryModel.CreateModel(driver, bus, loop, stops));
    }

    private List<Stop> GenerateStopList(Loop loop)
    {
        List<Stop> output = [];
        foreach(BusRoute route in loop.Routes)
        {
            BusRoute routeWithStop = _database.GetById<BusRoute>(route.Id, "Stop");
            if(routeWithStop.Stop == null)
            {
                continue;
            }
            output.Add(routeWithStop.Stop);
        }
        return output;
    }

    [HttpPost]
    public async Task<IActionResult> EntryForm([Bind("DriverId,BusId,LoopId,StopId,Boarded,LeftBehind")]LoopEntryModel model)
    {
        if(ModelState.IsValid)
        {
            await Task.Run(() => {
                int nextId = _database.GetAll<Entry>().Count + 1;
                _database.CreateEntity(new Entry(nextId, model.Boarded, model.LeftBehind)
                    .SetBus(_database.GetById<Bus>(model.BusId))
                    .SetDriver(_database.GetById<Driver>(model.DriverId))
                    .SetLoop(_database.GetById<Loop>(model.LoopId))
                    .SetStop(_database.GetById<Stop>(model.StopId)));
            });
        }
        return View(model);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
