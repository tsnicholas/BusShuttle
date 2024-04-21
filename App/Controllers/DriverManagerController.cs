using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using App.Service;
using Database.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class DriverManagerController(IAccountService accountService, IDatabaseService database) : Controller
{
    private readonly IAccountService _accountService = accountService;
    private readonly IDatabaseService _database = database;

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAll<Driver>().Select(driver => DriverViewModel.FromDriver(driver)));
    }

    [HttpGet]
    public IActionResult CreateDriver()
    {
        return View(CreateDriverModel.CreateDriver(_database.GenerateId<Driver>()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDriver([Bind("Id,FirstName,LastName,Email,Password")] CreateDriverModel driver)
    {
        if(!ModelState.IsValid) return View(driver);
        var result = await _accountService.CreateDriverAccount(driver.Email, driver.Password);
        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(driver);
        }
        await Task.Run(() => _database.CreateEntity(new Driver(driver.Id, driver.FirstName, driver.LastName, driver.Email)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> ActivateDriver([FromRoute] string id)
    {
        await _accountService.UpdateAccountActivation(id, true);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditDriver([FromRoute] int id)
    {
        Driver selectedDriver = _database.GetById<Driver>(id);
        return View(EditDriverModel.FromDriver(selectedDriver));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDriver(EditDriverModel editDriverModel)
    {
        if(!ModelState.IsValid) return View(editDriverModel);
        await Task.Run(() => {
            Driver updatedDriver = new(editDriverModel.Id, editDriverModel.FirstName, 
                editDriverModel.LastName, editDriverModel.Email);
            _database.UpdateById(editDriverModel.Id, updatedDriver);
        });
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
        await Task.Run(() => _database.DeleteById<Driver>(deleteDriverModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
