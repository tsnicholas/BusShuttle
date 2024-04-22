using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using Database.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class EntryManagerController(IDatabaseService database) : Controller
{
    private readonly IDatabaseService _database = database;

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAll<Entry>().Select(entry => EntryViewModel.FromEntry(entry)));
    }

    [HttpGet]
    public IActionResult CreateEntry()
    {
        int newId = _database.GenerateId<Entry>();
        List<Bus> buses = _database.GetAll<Bus>();
        List<Driver> drivers = _database.GetAll<Driver>();
        List<Stop> stops = _database.GetAll<Stop>();
        List<Loop> loops = _database.GetAll<Loop>();
        CreateEntryModel creationModel = CreateEntryModel.CreateEntry(
            newId, buses, drivers, loops, stops);
        return View(creationModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEntry(
        [Bind("Id,Timestamp,Boarded,LeftBehind,BusId,DriverId,LoopId,StopId")] CreateEntryModel entry)
    {
        if(!ModelState.IsValid) return View(entry);
        await Task.Run(() => {
            Entry newEntry = new(entry.Id, entry.Boarded, entry.LeftBehind);
            newEntry.SetBus(_database.GetById<Bus>(entry.BusId) ?? throw new InvalidOperationException());
            newEntry.SetDriver(_database.GetById<Driver>(entry.DriverId) ?? throw new InvalidOperationException());
            newEntry.SetLoop(_database.GetById<Loop>(entry.LoopId) ?? throw new InvalidOperationException());
            newEntry.SetStop(_database.GetById<Stop>(entry.StopId) ?? throw new InvalidOperationException());
            _database.CreateEntity(newEntry);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditEntry([FromRoute] int id)
    {
        Entry? selectedEntry = _database.GetById<Entry>(id);
        if(selectedEntry == null) return RedirectToAction("Index");
        List<Bus> buses = _database.GetAll<Bus>();
        List<Driver> drivers = _database.GetAll<Driver>();
        List<Stop> stops = _database.GetAll<Stop>();
        List<Loop> loops = _database.GetAll<Loop>();
        return View(EditEntryModel.FromEntry(selectedEntry, buses, drivers, loops, stops));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEntry(EditEntryModel viewModel)
    {
        if(!ModelState.IsValid) return View(viewModel);
        await Task.Run(() => {
            Entry updatedEntry = new Entry(viewModel.Id, viewModel.Boarded, viewModel.LeftBehind)
                .SetBus(_database.GetById<Bus>(viewModel.BusId) ?? throw new InvalidOperationException())
                .SetDriver(_database.GetById<Driver>(viewModel.DriverId) ?? throw new InvalidOperationException())
                .SetLoop(_database.GetById<Loop>(viewModel.LoopId) ?? throw new InvalidOperationException())
                .SetStop(_database.GetById<Stop>(viewModel.StopId) ?? throw new InvalidOperationException());
            _database.UpdateById(viewModel.Id, updatedEntry);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DeleteEntry([FromRoute] int id)
    {
        return View(DeleteEntryModel.DeleteEntry(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEntry(DeleteEntryModel deleteEntryModel)
    {
        if(!ModelState.IsValid) return View(deleteEntryModel);
        await Task.Run(() => _database.DeleteById<Entry>(deleteEntryModel.Id));
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
