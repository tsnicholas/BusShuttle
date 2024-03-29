using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Service;
using BusShuttleModel;

namespace App.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseService _service;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _service = new DatabaseService();
    }
    
    [Authorize("IsActivated")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult BusView()
    {
        return View(_service.GetAllBuses().Select(bus => BusViewModel.FromBus(bus)));
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult CreateBus()
    {
        return View(CreateBusModel.CreateBus(_service.GetAllBuses().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateBus([Bind("Id,BusNumber")] CreateBusModel bus)
    {
        if(!ModelState.IsValid) return View(bus);
        await Task.Run(() => _service.CreateBus(new Bus(bus.Id, bus.BusNumber)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult EditBus([FromRoute] int id)
    {
        Bus selectedBus = _service.GetBusById(id);
        return View(EditBusModel.FromBus(selectedBus));
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> EditBus(EditBusModel editBusModel)
    {
        if(!ModelState.IsValid) return View(editBusModel);
        await Task.Run(() => _service.EditBusById(editBusModel.Id, editBusModel.BusNumber));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
