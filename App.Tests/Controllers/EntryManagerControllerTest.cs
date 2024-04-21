using Microsoft.AspNetCore.Mvc;
using Moq;
using App.Controllers;
using BusShuttleModel;
using Database.Service;
using App.Models.Manager;
namespace App.Tests.Controllers;

public class EntryManagerControllerTest
{
    private static readonly string HomeAction = "Index";
    private static readonly string mockError = "I'm error";
    private static readonly List<Bus> testBuses = [ new(1, 69), new(2, 42) ];
    private static readonly List<Driver> testDrivers = [
        new(1, "Tim", "Nicholas", "tsnicholas@bsu.edu"),
        new(2, "Morgan", "Freeman", "MFreeman564@Gmail.com")
    ];
    private static readonly List<Loop> testLoops = [
        new(1, "Green Hill Zone"),
        new(2, "Casino Night Zone")
    ];
    private static readonly List<Stop> testStops = [
        new(1, "Aperture Science", -9999, 9999),
        new(2, "Black Mesa", 0, 0),
    ];
    private static readonly List<Entry> testEntries = [
        new Entry(1, 5, 8).SetBus(testBuses[0]).SetDriver(testDrivers[0]).SetLoop(testLoops[0]).SetStop(testStops[0]),
        new Entry(2, 10, 3).SetBus(testBuses[1]).SetDriver(testDrivers[1]).SetLoop(testLoops[1]).SetStop(testStops[1])
    ];
    private static readonly CreateEntryModel creationModel = new() {
        Id = testEntries.Count + 1, 
        Boarded = 1, 
        LeftBehind = 10, 
        BusId = testBuses[0].Id, 
        DriverId = testDrivers[0].Id, 
        StopId = testStops[1].Id,
        LoopId = testLoops[1].Id
    };
    private static readonly EditEntryModel editorModel = EditEntryModel.FromEntry(testEntries[0]);
    private static readonly DeleteEntryModel deletionModel = DeleteEntryModel.DeleteEntry(testEntries[1].Id);

    private static readonly Mock<IDatabaseService> mockDatabase = new();
    private readonly EntryManagerController _controller;

    public EntryManagerControllerTest()
    {
        _controller = new EntryManagerController(mockDatabase.Object);
        mockDatabase.Setup(x => x.GetAll<Bus>()).Returns(testBuses);
        foreach(Bus bus in testBuses)
        {
            mockDatabase.Setup(x => x.GetById<Bus>(bus.Id)).Returns(bus);
        }
        mockDatabase.Setup(x => x.GetAll<Driver>()).Returns(testDrivers);
        foreach(Driver driver in testDrivers)
        {
            mockDatabase.Setup(x => x.GetById<Driver>(driver.Id)).Returns(driver);
        }
        mockDatabase.Setup(x => x.GetAll<Stop>()).Returns(testStops);
        foreach(Stop stop in testStops)
        {
            mockDatabase.Setup(x => x.GetById<Stop>(stop.Id)).Returns(stop);
        }
        mockDatabase.Setup(x => x.GetAll<Loop>()).Returns(testLoops);
        foreach(Loop loop in testLoops)
        {
            mockDatabase.Setup(x => x.GetById<Loop>(loop.Id)).Returns(loop);
        }
        mockDatabase.Setup(x => x.GetAll<Entry>()).Returns(testEntries);
        foreach(Entry entry in testEntries)
        {
            mockDatabase.Setup(x => x.GetById<Entry>(entry.Id)).Returns(entry);
        }
        mockDatabase.Setup(x => x.GenerateId<Entry>()).Returns(testBuses.Count + 1);
    }

    [Fact]
    public void EntryManagerController_Index_SuccessfullyReturnsPage()
    {
        IEnumerable<EntryViewModel> entryViews = [
            EntryViewModel.FromEntry(testEntries[0]),
            EntryViewModel.FromEntry(testEntries[1])
        ];
        var result = (ViewResult) _controller.Index();
        Assert.Equivalent(entryViews, result.Model);
    }

    [Fact]
    public void EntryManagerController_CreateEntry_SuccessfullyReturnsPage()
    {
        CreateEntryModel initialModel = CreateEntryModel.CreateEntry(testEntries.Count + 1);
        var result = (ViewResult) _controller.CreateEntry();
        var resultingModel = result.Model as CreateEntryModel ?? throw new Exception("Model isn't the correct type.");
        // Since they were both created at different times, the timestamps won't be in sync.
        // To fix this problem, we simply make them equal for the sake of the unit test.
        initialModel.Timestamp = resultingModel.Timestamp;
        Assert.Equivalent(initialModel, resultingModel);
    }

    [Fact]
    public async void EntryManagerController_CreateEntry_ReturnsPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.CreateEntry(creationModel);
        Assert.Equal(creationModel, result.Model);
    }

    [Fact]
    public async void EntryManagerController_CreateEntry_SuccessfullyCreatesEntry()
    {
        Entry expectedEntry = new Entry(creationModel.Id, creationModel.Boarded, creationModel.LeftBehind)
            .SetBus(testBuses.Single(bus => bus.Id == creationModel.BusId))
            .SetDriver(testDrivers.Single(driver => driver.Id == creationModel.DriverId))
            .SetStop(testStops.Single(stop => stop.Id == creationModel.StopId))
            .SetLoop(testLoops.Single(loop => loop.Id == creationModel.LoopId));
        mockDatabase.Setup(x => x.CreateEntity(expectedEntry));
        var result = (RedirectToActionResult) await _controller.CreateEntry(creationModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void EntryManagerController_EditEntry_SuccessfullyReturnPage()
    {
        var result = (ViewResult) _controller.EditEntry(editorModel.Id);
        Assert.Equivalent(editorModel, result.Model);
    }

    [Fact]
    public async void EntryManagerController_EditEntry_ReturnPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.EditEntry(editorModel);
        Assert.Equal(editorModel, result.Model);
    }

    [Fact]
    public async void EntryManagerController_EditEntry_SuccessfullyEditEntry()
    {
        mockDatabase.Setup(x => x.UpdateById(editorModel.Id, testEntries[0]));
        var result = (RedirectToActionResult) await _controller.EditEntry(editorModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void EntryManagerController_DeleteEntry_SuccessfullyReturnPage()
    {
        var result = (ViewResult) _controller.DeleteEntry(deletionModel.Id);
        Assert.Equivalent(deletionModel, result.Model);
    }

    [Fact]
    public async void EntryManagerController_DeleteEntry_ReturnPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.DeleteEntry(deletionModel);
        Assert.Equal(deletionModel, result.Model);
    }

    [Fact]
    public async void EntryManagerController_DeleteEntry_SuccessfullyDeleteEntry()
    {
        mockDatabase.Setup(x => x.DeleteById<Entry>(deletionModel.Id));
        var result = (RedirectToActionResult) await _controller.DeleteEntry(deletionModel);
        Assert.Equal(HomeAction, result.ActionName);
    }
}