using Moq;
using Microsoft.AspNetCore.Mvc;
using App.Controllers;
using Database.Service;
using BusShuttleModel;
using App.Models.Manager;
namespace App.Tests.Controllers;

public class RouteManagerControllerTest
{
    private static readonly string HomeAction = "Index";
    private static readonly string mockError = "I'm error";
    private static readonly List<Stop> testStops = [
        new Stop(1, "Aperture Science", -9999, 9999),
        new Stop(2, "Black Mesa", 0, 0),
        new Stop(3, "Mushroom Kingdom", 500, 1000)
    ];
    private static readonly List<BusRoute> testRoutes = [
        new BusRoute(1, 2).SetStop(testStops[0]),
        new BusRoute(2, 3).SetStop(testStops[1])
    ];
    private static readonly CreateRouteModel creationModel = new()
    {
        Id = testRoutes.Count + 1, Order = 3, StopId = testStops[2].Id, Stops = testStops
    };
    private static readonly EditRouteModel editorModel = EditRouteModel.FromRoute(testRoutes[0]);
    private static readonly DeleteRouteModel deletionModel = DeleteRouteModel.DeleteRoute(testRoutes[1].Id);

    private static readonly Mock<IDatabaseService> mockDatabase = new();
    private readonly RouteManagerController _controller;

    public RouteManagerControllerTest()
    {
        _controller = new RouteManagerController(mockDatabase.Object);
        mockDatabase.Setup(x => x.GetAll<Stop>()).Returns(testStops);
        foreach(Stop stop in testStops.ToList())
        {
            mockDatabase.Setup(x => x.GetById<Stop>(stop.Id)).Returns(stop);
        }
        mockDatabase.Setup(x => x.GetAll<BusRoute>()).Returns(testRoutes);
        foreach(BusRoute route in testRoutes.ToList())
        {
            mockDatabase.Setup(x => x.GetById<BusRoute>(route.Id)).Returns(route);
        }
        mockDatabase.Setup(x => x.GenerateId<Stop>()).Returns(testStops.Count + 1);
        mockDatabase.Setup(x => x.GenerateId<BusRoute>()).Returns(testRoutes.Count + 1);
    }

    [Fact]
    public void RouteManagerController_Index_SuccessfullyReturnPage()
    {
        IEnumerable<RouteViewModel> viewModels = [
            RouteViewModel.FromRoute(testRoutes[0]),
            RouteViewModel.FromRoute(testRoutes[1])
        ];
        var result = (ViewResult) _controller.Index();
        Assert.Equivalent(viewModels, result.Model);
    }

    [Fact]
    public void RouteManagerController_CreateRoute_SuccessfullyReturnPage()
    {
        CreateRouteModel initialModel = CreateRouteModel.CreateRoute(testRoutes.Count + 1, testStops);
        var result = (ViewResult) _controller.CreateRoute();
        Assert.Equivalent(initialModel, result.Model);
    }

    [Fact]
    public async void RouteManagerController_CreateRoute_ReturnPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.CreateRoute(creationModel);
        Assert.Equal(creationModel, result.Model);
    }

    [Fact]
    public async void RouteManagerController_CreateRoute_SuccessfullyCreateRoute()
    {
        BusRoute expectedRoute = new BusRoute(creationModel.Id, creationModel.Order)
            .SetStop(creationModel.Stops.Single(stop => stop.Id == creationModel.StopId));
        mockDatabase.Setup(x => x.CreateEntity(expectedRoute));
        var result = (RedirectToActionResult) await _controller.CreateRoute(creationModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void RouteManagerController_EditRoute_SuccessfullyReturnPage()
    {
        var result = (ViewResult) _controller.EditRoute(editorModel.Id);
        Assert.Equivalent(editorModel, result.Model);
    }

    [Fact]
    public async void RouteManagerController_EditRoute_ReturnPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.EditRoute(editorModel);
        Assert.Equal(editorModel, result.Model);
    }

    [Fact]
    public async void RouteManagerController_EditRoute_SuccessfullyUpdateRoute()
    {
        BusRoute expectedRoute = new(editorModel.Id, editorModel.Order);
        mockDatabase.Setup(x => x.UpdateById(editorModel.Id, expectedRoute));
        var result = (RedirectToActionResult) await _controller.EditRoute(editorModel);
        Assert.Equal(HomeAction, result.ActionName);
    }


    [Fact]
    public void RouteManagerController_DeleteRoute_SuccessfullyReturnPage()
    {
        var result = (ViewResult) _controller.DeleteRoute(deletionModel.Id);
        Assert.Equivalent(deletionModel, result.Model);
    }

    [Fact]
    public async void RouteManagerController_DeleteRoute_ReturnPageOnModelError()
    {
        _controller.ModelState.AddModelError(string.Empty, mockError);
        var result = (ViewResult) await _controller.DeleteRoute(deletionModel);
        Assert.Equal(deletionModel, result.Model);
    }

    [Fact]
    public async void RouteManagerController_DeleteRoute_SuccessfullyDeleteRoute()
    {
        mockDatabase.Setup(x => x.DeleteById<BusRoute>(deletionModel.Id));
        var result = (RedirectToActionResult) await _controller.DeleteRoute(deletionModel);
        Assert.Equal(HomeAction, result.ActionName);
    }
}