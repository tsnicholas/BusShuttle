using Moq;
using Microsoft.AspNetCore.Mvc;
using App.Controllers;
using Database.Service;
using BusShuttleModel;
using App.Models.Manager;
namespace App.Tests.Controllers;

public class StopManagerControllerTest
{
    private static readonly string HomeAction = "Index";
    private static readonly string mockError = "I'm error";
    private static readonly List<Stop> testStops = [
        new Stop(1, "Aperture Science", -9999, 9999),
        new Stop(2, "Black Mesa", 0, 0),
    ];
    private static readonly CreateStopModel creationModel = new() {
        Id = testStops.Count + 1, Name = "Mushroom Kingdom", Latitude = 500, Longitude = 1000
    };
    private static readonly EditStopModel editorModel = EditStopModel.FromStop(testStops[0]);
    private static readonly DeleteStopModel deletionModel = DeleteStopModel.DeleteStop(testStops[0].Id);

    private static readonly Mock<IDatabaseService> mockDatabase = new();
    private readonly StopManagerController _controller;

    public StopManagerControllerTest()
    {
        _controller = new StopManagerController(mockDatabase.Object);
        mockDatabase.Setup(x => x.GetAll<Stop>()).Returns(testStops);
        mockDatabase.Setup(x => x.GetById<Stop>(testStops[0].Id)).Returns(testStops[0]);
        mockDatabase.Setup(x => x.GetById<Stop>(testStops[1].Id)).Returns(testStops[1]);
        mockDatabase.Setup(x => x.GenerateId<Stop>()).Returns(testStops.Count + 1);
    }

    [Fact]
    public void StopManagerController_Index_ReturnsPageSuccessfully()
    {
        IEnumerable<StopViewModel> viewModels = [
            StopViewModel.FromStop(testStops[0]),
            StopViewModel.FromStop(testStops[1])
        ];
        var result = (ViewResult) _controller.Index();
        Assert.Equivalent(viewModels, result.Model);
    }

    [Fact]
    public void StopManagerController_CreateStop_ReturnsPageSuccessfully()
    {
        CreateStopModel initialModel = CreateStopModel.CreateStop(testStops.Count + 1);
        var result = (ViewResult) _controller.CreateStop();
        Assert.Equivalent(initialModel, result.Model);
    }

    [Fact]
    public async void StopManagerController_CreateStop_ReturnsPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.CreateStop(creationModel);
        Assert.Equal(creationModel, result.Model);
    }

    [Fact]
    public async void StopManagerController_CreateStop_SuccessfullyCreatesStop()
    {
        Stop expectedStop = new (creationModel.Id, creationModel.Name, creationModel.Longitude, creationModel.Latitude);
        mockDatabase.Setup(x => x.CreateEntity(expectedStop));
        var result = (RedirectToActionResult) await _controller.CreateStop(creationModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void StopManagerController_EditStop_ReturnsPageSuccessfully()
    {
        var result = (ViewResult) _controller.EditStop(editorModel.Id);
        Assert.Equivalent(editorModel, result.Model);
    }

    [Fact]
    public async void StopManagerController_EditStop_ReturnsPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.EditStop(editorModel);
        Assert.Equal(editorModel, result.Model);
    }

    [Fact]
    public async void StopManagerController_EditStop_SuccessfullyUpdatesStop()
    {
        Stop expectedStop = new(editorModel.Id, editorModel.Name, editorModel.Longitude, editorModel.Latitude);
        mockDatabase.Setup(x => x.UpdateById(editorModel.Id, expectedStop));
        var result = (RedirectToActionResult) await _controller.EditStop(editorModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void StopManagerController_DeleteStop_ReturnPageSuccessfully()
    {
        var result = (ViewResult) _controller.DeleteStop(deletionModel.Id);
        Assert.Equivalent(deletionModel, result.Model);
    }

    [Fact]
    public async void StopManagerController_DeleteStop_ReturnPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, "I'm in your walls.");
        var result = (ViewResult) await _controller.DeleteStop(deletionModel);
        Assert.Equal(deletionModel, result.Model);
    }

    [Fact]
    public async void StopManagerController_DeleteStop_DeleteDriverSuccessfully()
    {
        mockDatabase.Setup(x => x.DeleteById<Driver>(deletionModel.Id));
        var result = (RedirectToActionResult) await _controller.DeleteStop(deletionModel);
        Assert.Equal(HomeAction, result.ActionName);
    }
}