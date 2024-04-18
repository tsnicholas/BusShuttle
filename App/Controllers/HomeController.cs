using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using App.Service;
using App.Models;
using App.Models.DriverModels;
using BusShuttleModel;
namespace App.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if(User.IsInRole("Manager"))
        {
            return RedirectToAction("Manager");
        }
        return RedirectToAction("Index", "Driver");
    }
    
    [Authorize(Roles = "Manager")]
    public IActionResult Manager()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize("IsActivated")]
    public async Task<IActionResult> Driver(DriverHomeModel model)
    {
        if(!ModelState.IsValid)
        {
            return RedirectToAction("Index", "BusManager");
        }
        var routeDictionary = new RouteValueDictionary();
        await Task.Run(() => {
            routeDictionary.Add("busId", model.BusId);
            routeDictionary.Add("loopId", model.LoopId);
        });
        return RedirectToAction("Index", "BusManager");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
