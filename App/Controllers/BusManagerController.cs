using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class BusManagerController : Controller 
{
    private readonly ILogger<BusManagerController> _logger;
    private readonly DatabaseService _database;

    public BusManagerController(ILogger<BusManagerController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAllBuses().Select(bus => BusViewModel.FromBus(bus)));
    }

    [HttpGet]
    public IActionResult CreateBus()
    {
        return View(CreateBusModel.CreateBus(_database.GetAllBuses().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBus([Bind("Id,BusNumber")] CreateBusModel bus)
    {
        if(!ModelState.IsValid) return View(bus);
        await Task.Run(() => _database.CreateBus(new Bus(bus.Id, bus.BusNumber)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditBus([FromRoute] int id)
    {
        Bus selectedBus = _database.GetBusById(id);
        return View(EditBusModel.FromBus(selectedBus));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBus(EditBusModel editBusModel)
    {
        if(!ModelState.IsValid) return View(editBusModel);
        await Task.Run(() => _database.EditBusById(editBusModel.Id, editBusModel.BusNumber));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DeleteBus([FromRoute] int id)
    {
        return View(DeleteBusModel.DeleteBus(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBus(DeleteBusModel deleteBusModel)
    {
        if(!ModelState.IsValid) return View(deleteBusModel);
        await Task.Run(() => _database.DeleteBusById(deleteBusModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
