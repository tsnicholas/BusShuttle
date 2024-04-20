using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
namespace App.Controllers;

[Authorize]
public class HomeController() : Controller
{
    public IActionResult Index()
    {
        var user = Thread.CurrentPrincipal ?? throw new InvalidDataException();
        if(user.IsInRole("Manager"))
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
