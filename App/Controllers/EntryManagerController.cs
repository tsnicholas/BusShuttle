using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Models.Manager;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class EntryManagerController : Controller
{
    private readonly ILogger<EntryManagerController> _logger;
    private readonly IDatabaseService _database;

    public EntryManagerController(ILogger<EntryManagerController> logger, IDatabaseService database)
    {
        _logger = logger;
        _database = database;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAll<Entry>().Select(entry => EntryViewModel.FromEntry(entry)));
    }

    [HttpGet]
    public IActionResult CreateEntry()
    {
        return View(CreateEntryModel.CreateEntry(_database.GetAll<Entry>().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEntry(
        [Bind("Id,Timestamp,Boarded,LeftBehind,BusId,DriverId,LoopId,StopId")] CreateEntryModel entry)
    {
        if(!ModelState.IsValid) return View(entry);
        await Task.Run(() => {
            Entry newEntry = new Entry(entry.Id, entry.Boarded, entry.LeftBehind);
            newEntry.SetBus(_database.GetById<Bus>(entry.BusId));
            newEntry.SetDriver(_database.GetById<Driver>(entry.DriverId));
            newEntry.SetLoop(_database.GetById<Loop>(entry.LoopId));
            newEntry.SetStop(_database.GetById<Stop>(entry.StopId));
            _database.CreateEntity<Entry>(newEntry);
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditEntry([FromRoute] int id)
    {
        Entry selectedEntry = _database.GetById<Entry>(id);
        return View(EditEntryModel.FromEntry(selectedEntry));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEntry(EditEntryModel viewModel)
    {
        if(!ModelState.IsValid) return View(viewModel);
        await Task.Run(() => {
            Entry updatedEntry = new Entry(viewModel.Id, viewModel.Boarded, viewModel.LeftBehind);
            updatedEntry.SetBus(_database.GetById<Bus>(viewModel.BusId));
            updatedEntry.SetDriver(_database.GetById<Driver>(viewModel.DriverId));
            updatedEntry.SetLoop(_database.GetById<Loop>(viewModel.LoopId));
            updatedEntry.SetStop(_database.GetById<Stop>(viewModel.StopId));
            _database.UpdateById<Entry>(viewModel.Id, updatedEntry);
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
