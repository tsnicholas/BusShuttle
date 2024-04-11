using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class DriverManagerController : Controller
{
    private readonly ILogger<DriverManagerController> _logger;
    private readonly DatabaseService _database;

    public DriverManagerController(ILogger<DriverManagerController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAllDrivers().Select(driver => DriverViewModel.FromDriver(driver)));
    }

    [HttpGet]
    public IActionResult CreateDriver()
    {
        return View(CreateDriverModel.CreateDriver(_database.GetAllDrivers().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDriver([Bind("Id,FirstName,LastName")] CreateDriverModel driver)
    {
        if(!ModelState.IsValid) return View(driver);
        await Task.Run(() => _database.CreateDriver(new Driver(driver.Id, driver.FirstName, driver.LastName)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditDriver([FromRoute] int id)
    {
        Driver selectedDriver = _database.GetDriverById(id);
        return View(EditDriverModel.FromDriver(selectedDriver));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDriver(EditDriverModel editDriverModel)
    {
        if(!ModelState.IsValid) return View(editDriverModel);
        await Task.Run(() => _database.EditDriverById(editDriverModel.Id, editDriverModel.FirstName, editDriverModel.LastName));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DeleteDriver([FromRoute] int id)
    {
        return View(DeleteDriverModel.DeleteDriver(id));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteDriver(DeleteDriverModel deleteDriverModel)
    {
        if(!ModelState.IsValid) return View(deleteDriverModel);
        await Task.Run(() => _database.DeleteDriverById(deleteDriverModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
