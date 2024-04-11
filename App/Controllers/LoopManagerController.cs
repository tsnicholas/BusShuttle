using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class LoopManagerController : Controller
{
    public readonly ILogger<LoopManagerController> _logger;
    public readonly DatabaseService _database;

    public LoopManagerController(ILogger<LoopManagerController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    [HttpGet]
    public IActionResult LoopView()
    {
        return View(_database.GetAllLoops().Select(loop => LoopViewModel.FromLoop(loop)));
    }

    [HttpGet]
    public IActionResult CreateLoop()
    {
        return View(CreateLoopModel.CreateLoop(_database.GetAllLoops().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLoop([Bind("Id,Name")] CreateLoopModel loop)
    {
        if(!ModelState.IsValid) return View(loop);
        await Task.Run(() => _database.CreateLoop(new Loop(loop.Id, loop.Name)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditLoop([FromRoute] int id)
    {
        Loop selectedLoop = _database.GetLoopWithId(id);
        return View(EditLoopModel.FromLoop(selectedLoop));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLoop(EditLoopModel editLoopModel)
    {
        if(!ModelState.IsValid) return View(editLoopModel);
        await Task.Run(() => _database.EditLoopWithId(editLoopModel.Id, editLoopModel.Name));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DeleteLoop([FromRoute] int id)
    {
        return View(DeleteLoopModel.DeleteLoop(id));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteLoop(DeleteLoopModel deleteLoopModel)
    {
        if(!ModelState.IsValid) return View(deleteLoopModel);
        await Task.Run(() => _database.DeleteLoopWithId(deleteLoopModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
