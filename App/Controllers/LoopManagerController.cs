using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using Database.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class LoopManagerController(IDatabaseService database) : Controller
{
    private readonly IDatabaseService _database = database;

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAll<Loop>().Select(loop => LoopViewModel.FromLoop(loop)));
    }

    [HttpGet]
    public IActionResult CreateLoop()
    {
        return View(CreateLoopModel.CreateLoop(_database.GenerateId<Loop>()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLoop([Bind("Id,Name")] CreateLoopModel loop)
    {
        if(!ModelState.IsValid) return View(loop);
        await Task.Run(() => _database.CreateEntity(new Loop(loop.Id, loop.Name)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditLoop([FromRoute] int id)
    {
        Loop? selectedLoop = _database.GetById<Loop>(id);
        if(selectedLoop == null) return RedirectToAction("Index");
        return View(EditLoopModel.FromLoop(selectedLoop));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLoop(EditLoopModel editLoopModel)
    {
        if(!ModelState.IsValid) return View(editLoopModel);
        await Task.Run(() => {
            Loop updatedLoop = new(editLoopModel.Id, editLoopModel.Name);
            _database.UpdateById(editLoopModel.Id, updatedLoop);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditRoutesInLoop([FromRoute] int id)
    {
        Loop? loop = _database.GetById<Loop>(id);
        if(loop == null) return RedirectToAction("Index");
        return View(EditRoutesInLoopModel.FromLoop(id, loop.Routes));
    }

    [HttpGet]
    public IActionResult AddRoute([FromRoute] int id)
    {
        List<BusRoute> routes = _database.GetAll<BusRoute>();
        return View(AddRouteToLoopModel.FromId(id, routes));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRoute([Bind("LoopId,RouteId")] AddRouteToLoopModel model)
    {
        if(!ModelState.IsValid) return View(model);
        BusRoute? route = _database.GetById<BusRoute>(model.RouteId);
        if(route == null) return View(model);
        await Task.Run(() => {
            Loop? loop = _database.GetById<Loop>(model.LoopId) ?? throw new InvalidOperationException();
            route.SetLoop(loop);
            loop.AddRoute(route);
            _database.SaveChanges();
        });
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
        await Task.Run(() => _database.DeleteById<Loop>(deleteLoopModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
