using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using Database.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class RouteManagerController : Controller
{
    private readonly ILogger<RouteManagerController> _logger;
    private readonly IDatabaseService _database;

    public RouteManagerController(ILogger<RouteManagerController> logger, IDatabaseService database)
    {
        _logger = logger;
        _database = database;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAll<BusRoute>().Select(route => RouteViewModel.FromRoute(route)));
    }

    [HttpGet]
    public IActionResult CreateRoute()
    {
        List<Stop> stops = _database.GetAll<Stop>();
        foreach(var stop in stops)
        {
            if(stop.Route != null)
            {
                stops.Remove(stop);
            }
        }
        return View(CreateRouteModel.CreateRoute(_database.GetAll<BusRoute>().Count() + 1, stops));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoute([Bind("Id,Order,StopId")] CreateRouteModel createdRoute)
    {
        if(!ModelState.IsValid) return View(createdRoute);
        await Task.Run(() => {
            Stop stop = _database.GetById<Stop>(createdRoute.StopId);
            BusRoute newRoute = new BusRoute(createdRoute.Id, createdRoute.Order);
            newRoute.SetStop(stop);
            stop.SetRoute(newRoute);
            _database.CreateEntity<BusRoute>(newRoute);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditRoute([FromRoute] int id)
    {
        BusRoute selectedRoute = _database.GetById<BusRoute>(id);
        return View(EditRouteModel.FromRoute(selectedRoute));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoute(EditRouteModel editRouteModel)
    {
        if(!ModelState.IsValid) return View(editRouteModel);
        await Task.Run(() => {
            BusRoute updatedRoute = new BusRoute(editRouteModel.Id, editRouteModel.Order);
            _database.UpdateById<BusRoute>(editRouteModel.Id, updatedRoute);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DeleteRoute([FromRoute] int id)
    {
        return View(DeleteRouteModel.DeleteRoute(id));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRoute(DeleteRouteModel deleteRouteModel)
    {
        if(!ModelState.IsValid) return View(deleteRouteModel);
        await Task.Run(() => {
            _database.DeleteById<BusRoute>(deleteRouteModel.Id);
        });
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
