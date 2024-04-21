using App.Controllers;
using App.Models.DriverModels;
using App.Service;
using BusShuttleModel;
using Database.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
namespace App.Tests.Controllers;



public class DriverControllerTests
{
    private static readonly List<Loop> testLoops = [
        new Loop(1, "Green Hill Zone"),
        new Loop(2, "Mushroom Kingdom")
    ];
    private static readonly List<Bus> testBuses = [new Bus(1, 69), new Bus(2, 420)];
    private static readonly Driver testDriver = new(1, "Tim", "Nicholas", "tsnicholas@bsu.edu");
    private static readonly DriverHomeModel mockHomeModel = DriverHomeModel.CreateUsingLists(testLoops, testBuses);
    private static readonly Stop mockStop = new(1, "Bikini Bottom", 500, -1000);
    private static readonly BusRoute mockRoute = new BusRoute(1, 2).SetStop(mockStop);
    private static readonly LoopEntryModel mockEntryModel = LoopEntryModel.CreateModel(
        testDriver, testBuses[0], testLoops[0], [mockStop]);

    private static readonly Mock<IAccountService> accountService = new();
    private static readonly Mock<IDatabaseService> databaseService = new();
    private readonly DriverController controller;

    public DriverControllerTests()
    {
        controller = new DriverController(accountService.Object, databaseService.Object);
    }

    [Fact]
    public void DriverController_Index_ReturnsHomeScreen()
    {
        databaseService.Setup(x => x.GetAll<Loop>()).Returns(testLoops);
        databaseService.Setup(x => x.GetAll<Bus>()).Returns(testBuses);
        ViewResult result = (ViewResult) controller.Index();
        Assert.Equivalent(mockHomeModel, result.Model);
    }

    [Fact]
    public async void DriverController_Index_InvalidPostRequestRejected()
    {
        controller.ModelState.AddModelError("key", "I'm broken :)");
        ViewResult result = (ViewResult) await controller.Index(mockHomeModel);
        Assert.Equivalent(mockHomeModel, result.Model);
    }

    [Fact]
    public async void DriverController_Index_PostRequestRedirectsSuccessfully()
    {
        RedirectToActionResult result = (RedirectToActionResult) await controller.Index(mockHomeModel);
        Assert.Equal("EntryForm", result.ActionName);
        var expectedRoute = new RouteValueDictionary
        {
            { "busId", mockHomeModel.BusId },
            { "loopId", mockHomeModel.LoopId }
        };
        Assert.Equivalent(expectedRoute, result.RouteValues);
    }

    [Fact]
    public async void DriverController_EntryForm_ReturnsViewSuccessfully()
    {
        string mockEmail = testDriver.Email;
        accountService.Setup(x => x.GetCurrentEmail()).Returns(Task.FromResult(mockEmail));
        databaseService.Setup(x => x.GetDriverByEmail(mockEmail)).Returns(testDriver);
        
        Bus mockBus = testBuses[0];
        databaseService.Setup(x => x.GetById<Bus>(mockHomeModel.BusId)).Returns(mockBus);

        Loop mockLoop = testLoops[0];
        mockLoop.AddRoute(mockRoute);
        databaseService.Setup(x => x.GetById<Loop>(mockHomeModel.LoopId, "BusRoute")).Returns(mockLoop);
        databaseService.Setup(x => x.GetById<BusRoute>(mockRoute.Id, "Stop")).Returns(mockRoute);
        
        var result = (ViewResult) await controller.EntryForm(mockHomeModel.BusId, mockHomeModel.LoopId);
        Assert.Equivalent(LoopEntryModel.CreateModel(testDriver, mockBus, mockLoop, [mockStop]), result.Model);
    }

    [Fact]
    public async void DriverController_EntryForm_ReturnsDuringModelStateError()
    {
        controller.ModelState.AddModelError("key", "Fix me :(");
        var result = (ViewResult) await controller.EntryForm(mockEntryModel);
        Assert.Equivalent(mockEntryModel, result.Model);
    }

    [Fact]
    public async void DriverController_EntryForm_SubmitsDataNormally()
    {
        Bus mockBus = testBuses[0];
        Loop mockLoop = testLoops[0];
        databaseService.Setup(x => x.GetAll<Entry>()).Returns([]);
        databaseService.Setup(x => x.GetById<Bus>(mockBus.Id)).Returns(mockBus);
        databaseService.Setup(x => x.GetById<Driver>(testDriver.Id)).Returns(testDriver);
        databaseService.Setup(x => x.GetById<Stop>(mockStop.Id)).Returns(mockStop);
        databaseService.Setup(x => x.GetById<Loop>(mockLoop.Id)).Returns(mockLoop);
        Entry createdEntry = new Entry(1, mockEntryModel.Boarded, mockEntryModel.LeftBehind)
            .SetBus(mockBus).SetDriver(testDriver).SetStop(mockStop).SetLoop(mockLoop);
        databaseService.Setup(x => x.CreateEntity(createdEntry));
        var result = (ViewResult) await controller.EntryForm(mockEntryModel);
        Assert.Equivalent(mockEntryModel, result.Model);
    }
}
