using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class RouteManagerController : Controller
{
    public readonly ILogger<RouteManagerController> _logger;
    public readonly DatabaseService _database;

    public RouteManagerController(ILogger<RouteManagerController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAllRoutes().Select(route => RouteViewModel.FromRoute(route)));
    }

    [HttpGet]
    public IActionResult CreateRoute()
    {
        List<Stop> stops = _database.GetAllStops();
        foreach(var stop in stops)
        {
            if(stop.Route != null)
            {
                stops.Remove(stop);
            }
        }
        return View(CreateRouteModel.CreateRoute(_database.GetAllRoutes().Count() + 1, stops));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoute([Bind("Id,Order,StopId")] CreateRouteModel createdRoute)
    {
        if(!ModelState.IsValid) return View(createdRoute);
        await Task.Run(() => {
            Stop stop = _database.GetStopById(createdRoute.StopId);
            BusRoute newRoute = new BusRoute(createdRoute.Id, createdRoute.Order, stop);
            stop.SetRoute(newRoute);
            _database.CreateRoute(newRoute);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditRoute([FromRoute] int id)
    {
        BusRoute selectedRoute = _database.GetRouteById(id);
        return View(EditRouteModel.FromRoute(selectedRoute));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoute(EditRouteModel editRouteModel)
    {
        if(!ModelState.IsValid) return View(editRouteModel);
        await Task.Run(() => _database.EditRouteById(editRouteModel.Id, editRouteModel.Order));
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
        await Task.Run(() => _database.DeleteRouteById(deleteRouteModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
