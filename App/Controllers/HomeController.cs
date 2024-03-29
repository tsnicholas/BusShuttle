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
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> EditBus(EditBusModel editBusModel)
    {
        if(!ModelState.IsValid) return View(editBusModel);
        await Task.Run(() => _service.EditBusById(editBusModel.Id, editBusModel.BusNumber));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult DeleteBus([FromRoute] int id)
    {
        return View(DeleteBusModel.DeleteBus(id));
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteBus(DeleteBusModel deleteBusModel)
    {
        if(!ModelState.IsValid) return View(deleteBusModel);
        await Task.Run(() => _service.DeleteBusById(deleteBusModel.Id));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult DriverView()
    {
        return View(_service.GetAllDrivers().Select(driver => DriverViewModel.FromDriver(driver)));
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult CreateDriver()
    {
        return View(CreateDriverModel.CreateDriver(_service.GetAllDrivers().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateDriver([Bind("Id,FirstName,LastName")] CreateDriverModel driver)
    {
        if(!ModelState.IsValid) return View(driver);
        await Task.Run(() => _service.CreateDriver(new Driver(driver.Id, driver.FirstName, driver.LastName)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult EditDriver([FromRoute] int id)
    {
        Driver selectedDriver = _service.GetDriverById(id);
        return View(EditDriverModel.FromDriver(selectedDriver));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> EditDriver(EditDriverModel editDriverModel)
    {
        if(!ModelState.IsValid) return View(editDriverModel);
        await Task.Run(() => _service.EditDriverById(editDriverModel.Id, editDriverModel.FirstName, editDriverModel.LastName));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult DeleteDriver([FromRoute] int id)
    {
        return View(DeleteDriverModel.DeleteDriver(id));
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteDriver(DeleteDriverModel deleteDriverModel)
    {
        if(!ModelState.IsValid) return View(deleteDriverModel);
        await Task.Run(() => _service.DeleteDriverById(deleteDriverModel.Id));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult RouteView()
    {
        return View(_service.GetAllRoutes().Select(route => RouteViewModel.FromRoute(route)));
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult CreateRoute()
    {
        return View(CreateRouteModel.CreateRoute(_service.GetAllRoutes().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateRoute([Bind("Id,Order")] CreateRouteModel route)
    {
        if(!ModelState.IsValid) return View(route);
        await Task.Run(() => _service.CreateRoute(new BusShuttleModel.Route(route.Id, route.Order)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult EditRoute([FromRoute] int id)
    {
        BusShuttleModel.Route selectedRoute = _service.GetRouteById(id);
        return View(EditRouteModel.FromRoute(selectedRoute));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> EditRoute(EditRouteModel editRouteModel)
    {
        if(!ModelState.IsValid) return View(editRouteModel);
        await Task.Run(() => _service.EditRouteById(editRouteModel.Id, editRouteModel.Order));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult DeleteRoute([FromRoute] int id)
    {
        return View(DeleteRouteModel.DeleteRoute(id));
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteRoute(DeleteRouteModel deleteRouteModel)
    {
        if(!ModelState.IsValid) return View(deleteRouteModel);
        await Task.Run(() => _service.DeleteRouteById(deleteRouteModel.Id));
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult StopView()
    {
        return View(_service.GetAllStops().Select(stop => StopViewModel.FromStop(stop)));
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult CreateStop()
    {
        return View(CreateStopModel.CreateStop(_service.GetAllStops().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateStop([Bind("Id,Name,Latitude,Longitude,RouteId")] CreateStopModel stop)
    {
        if(!ModelState.IsValid) return View(stop);
        await Task.Run(() => _service.CreateStop(new Stop(stop.Id, stop.Name, stop.Latitude, stop.Longitude, stop.RouteId)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult EditStop([FromRoute] int id)
    {
        Stop selectedStop = _service.GetStopById(id);
        return View(EditStopModel.FromStop(selectedStop));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> EditStop(EditStopModel editStopModel)
    {
        if(!ModelState.IsValid) return View(editStopModel);
        await Task.Run(() => _service.EditStopById(editStopModel.Id, editStopModel.Name, editStopModel.Latitude, editStopModel.Longitude, editStopModel.RouteId));
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult DeleteStop([FromRoute] int id)
    {
        return View(DeleteStopModel.DeleteStop(id));
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteStop(DeleteStopModel deleteStopModel)
    {
        if(!ModelState.IsValid) return View(deleteStopModel);
        await Task.Run(() => _service.DeleteStopById(deleteStopModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
