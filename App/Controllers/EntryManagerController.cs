using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using App.Service;
using BusShuttleModel;
namespace App.Controllers;

[Authorize(Roles = "Manager")]
public class EntryManagerController : Controller
{
    private readonly ILogger<EntryManagerController> _logger;
    private readonly DatabaseService _database;

    public EntryManagerController(ILogger<EntryManagerController> logger)
    {
        _logger = logger;
        _database = new DatabaseService();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_database.GetAllEntries().Select(entry => EntryViewModel.FromEntry(entry)));
    }

    [HttpGet]
    public IActionResult CreateEntry()
    {
        return View(CreateEntryModel.CreateEntry(_database.GetAllEntries().Count() + 1));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEntry([Bind("Id,Timestamp,Boarded,LeftBehind,BusId,DriverId,LoopId,StopId")] CreateEntryModel entry)
    {
        if(!ModelState.IsValid) return View(entry);
        await Task.Run(() => _database.CreateEntry(new Entry(entry.Id, entry.Boarded, entry.LeftBehind, entry.BusId, entry.DriverId, entry.LoopId, entry.StopId)));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditEntry([FromRoute] int id)
    {
        Entry selectedEntry = _database.GetEntryWithId(id);
        return View(EditEntryModel.FromEntry(selectedEntry));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEntry(EditEntryModel editEntryModel)
    {
        if(!ModelState.IsValid) return View(editEntryModel);
        await Task.Run(() => _database.EditEntryWithId(editEntryModel.Id, editEntryModel.Timestamp, editEntryModel.Boarded, editEntryModel.LeftBehind, editEntryModel.BusId, editEntryModel.DriverId, editEntryModel.LoopId, editEntryModel.StopId));
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
        await Task.Run(() => _database.DeleteEntryWithId(deleteEntryModel.Id));
        return RedirectToAction("Index");
    }
}
