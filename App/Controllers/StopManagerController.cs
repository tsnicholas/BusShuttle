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
    public readonly ILogger<StopManagerController> _logger;
    public readonly DatabaseService _database;

    public StopManagerController(ILogger<StopManagerController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAllStops().Select(stop => StopViewModel.FromStop(stop)));
    }

    [HttpGet]
    public IActionResult CreateStop()
    {
        return View(CreateStopModel.CreateStop(_database.GetAllStops().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStop([Bind("Id,Name,Latitude,Longitude")] CreateStopModel stop)
    {
        if(!ModelState.IsValid) return View(stop);
        await Task.Run(() => _database.CreateStop(new Stop(stop.Id, stop.Name, stop.Latitude, stop.Longitude)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditStop([FromRoute] int id)
    {
        Stop selectedStop = _database.GetStopById(id);
        return View(EditStopModel.FromStop(selectedStop));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStop(EditStopModel editStopModel)
    {
        if(!ModelState.IsValid) return View(editStopModel);
        await Task.Run(() => _database.EditStopById(editStopModel.Id, editStopModel.Name, editStopModel.Latitude, editStopModel.Longitude));
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
        await Task.Run(() => _database.DeleteStopById(deleteStopModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
