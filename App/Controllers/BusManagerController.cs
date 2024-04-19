using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using Database.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class BusManagerController : Controller 
{
    private readonly ILogger<BusManagerController> _logger;
    private readonly IDatabaseService _database;

    public BusManagerController(ILogger<BusManagerController> logger, IDatabaseService database)
    {
        _logger = logger;
        _database = database;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAll<Bus>().Select(bus => BusViewModel.FromBus(bus)));
    }

    [HttpGet]
    public IActionResult CreateBus()
    {
        return View(CreateBusModel.CreateBus(_database.GetAll<Bus>().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBus([Bind("Id,BusNumber")] CreateBusModel viewModel)
    {
        if(!ModelState.IsValid) return View(viewModel);
        await Task.Run(() => _database.CreateEntity<Bus>(new Bus(viewModel.Id, viewModel.BusNumber)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditBus([FromRoute] int id)
    {
        Bus selectedBus = _database.GetById<Bus>(id);
        return View(EditBusModel.FromBus(selectedBus));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBus(EditBusModel editBusModel)
    {
        if(!ModelState.IsValid) return View(editBusModel);
        await Task.Run(() => {
            Bus updatedBus = new Bus(editBusModel.Id, editBusModel.BusNumber);
            _database.UpdateById<Bus>(editBusModel.Id, updatedBus);
        });
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
        await Task.Run(() => _database.DeleteById<Bus>(deleteBusModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
