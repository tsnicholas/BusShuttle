using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class StopManagerController : Controller
{
    private readonly ILogger<StopManagerController> _logger;
    private readonly IDatabaseService _database;

    public StopManagerController(ILogger<StopManagerController> logger, IDatabaseService database)
    {
        _logger = logger;
        _database = database;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAll<Stop>().Select(stop => StopViewModel.FromStop(stop)));
    }

    [HttpGet]
    public IActionResult CreateStop()
    {
        return View(CreateStopModel.CreateStop(_database.GetAll<Stop>().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStop([Bind("Id,Name,Latitude,Longitude")] CreateStopModel stop)
    {
        if(!ModelState.IsValid) return View(stop);
        await Task.Run(() => _database.CreateEntity<Stop>(new Stop(stop.Id, stop.Name, stop.Latitude, stop.Longitude)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditStop([FromRoute] int id)
    {
        Stop selectedStop = _database.GetById<Stop>(id);
        return View(EditStopModel.FromStop(selectedStop));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStop(EditStopModel editStopModel)
    {
        if(!ModelState.IsValid) return View(editStopModel);
        await Task.Run(() => {
            Stop updatedStop = new Stop(editStopModel.Id, editStopModel.Name, editStopModel.Longitude, editStopModel.Latitude);
            _database.UpdateById<Stop>(editStopModel.Id, updatedStop);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DeleteStop([FromRoute] int id)
    {
        return View(DeleteStopModel.DeleteStop(id));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteStop(DeleteStopModel deleteStopModel)
    {
        if(!ModelState.IsValid) return View(deleteStopModel);
        await Task.Run(() => _database.DeleteById<Stop>(deleteStopModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
