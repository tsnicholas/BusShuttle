using App.Controllers;
using App.Models.Manager;
using BusShuttleModel;
using Database.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace App.Tests.Controllers;

public class LoopManagerControllerTests
{
    private static readonly string HomeAction = "Index";
    private static readonly List<Loop> testLoops = [
        new Loop(1, "Green Hill Zone"),
        new Loop(2, "Casino Night Zone")
    ];
    private static readonly int mockRandomResult = 999;
    private static readonly List<BusRoute> testRoutes = [
        new BusRoute(1, 1).SetStop(new Stop(1, "Bikini Bottom", 500, -400)), 
        new BusRoute(2, 10).SetStop(new Stop(2, "South Park", 100, 30))
    ];
    private static readonly CreateLoopModel creationModel = new()
    {
        Id = mockRandomResult, Name = "Emerald Hill Zone"
    };
    private static readonly EditLoopModel editModel = EditLoopModel.FromLoop(testLoops[0]);
    private static readonly AddRouteToLoopModel routeAdditionModel = AddRouteToLoopModel.FromId(testLoops[1].Id, testRoutes);
    private static readonly DeleteLoopModel deletionModel = DeleteLoopModel.DeleteLoop(testLoops[0].Id);

    private static readonly Mock<IDatabaseService> mockService = new();
    private readonly LoopManagerController controller;

    public LoopManagerControllerTests()
    {
        controller = new LoopManagerController(mockService.Object);
        mockService.Setup(x => x.GetAll<Loop>()).Returns(testLoops);
        mockService.Setup(x => x.GetById<Loop>(testLoops[0].Id)).Returns(testLoops[0]);
        mockService.Setup(x => x.GetById<Loop>(testLoops[1].Id)).Returns(testLoops[1]);
        mockService.Setup(x => x.GenerateId<Loop>()).Returns(mockRandomResult);
    }

    [Fact]
    public void LoopManagerController_Index_ReturnPageSuccessfully()
    {
        IEnumerable<LoopViewModel> expectedModels = [
            LoopViewModel.FromLoop(testLoops[0]),
            LoopViewModel.FromLoop(testLoops[1])
        ];
        var result = (ViewResult) controller.Index();
        Assert.Equivalent(expectedModels, result.Model);
    }

    [Fact]
    public void LoopManagerController_CreateLoop_ReturnPageSuccessfully()
    {
        CreateLoopModel initialModel = CreateLoopModel.CreateLoop(mockRandomResult);
        var result = (ViewResult) controller.CreateLoop();
        Assert.Equivalent(initialModel, result.Model);
    }

    [Fact]
    public async void LoopManagerController_CreateLoop_ReturnPageOnModelError()
    {
        controller.ModelState.AddModelError(string.Empty, string.Empty);
        var result = (ViewResult) await controller.CreateLoop(creationModel);
        Assert.Equal(creationModel, result.Model);
    }

    [Fact]
    public async void LoopManagerController_CreateLoop_SuccessfullyCreatesLoop()
    {
        Loop expectedLoop = new(creationModel.Id, creationModel.Name);
        mockService.Setup(x => x.CreateEntity(expectedLoop));
        var result = (RedirectToActionResult) await controller.CreateLoop(creationModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void LoopManagerController_EditLoop_ReturnPageSuccessfully()
    {
        var result = (ViewResult) controller.EditLoop(editModel.Id);
        Assert.Equivalent(editModel, result.Model);
    }

    [Fact]
    public async void LoopManagerController_EditLoop_ReturnPageOnModelError()
    {
        controller.ModelState.AddModelError(string.Empty, "error description");
        var result = (ViewResult) await controller.EditLoop(editModel);
        Assert.Equal(editModel, result.Model);
    }

    [Fact]
    public async void LoopManagerController_EditLoop_UpdatesLoopSuccessfully()
    {
        mockService.Setup(x => x.UpdateById(editModel.Id, new Loop(editModel.Id, editModel.Name)));
        var result = (RedirectToActionResult) await controller.EditLoop(editModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void LoopManagerController_EditRoutesInLoop_ReturnPageSuccessfully()
    {
        EditRoutesInLoopModel routeEditorModel = EditRoutesInLoopModel.FromLoop(
            testLoops[1].Id, testLoops[1].Routes);
        var result = (ViewResult) controller.EditRoutesInLoop(routeEditorModel.LoopId);
        Assert.Equivalent(routeEditorModel, result.Model);
    }

    [Fact]
    public void LoopManagerController_AddRoute_ReturnPageSuccessfully()
    {
        mockService.Setup(x => x.GetAll<BusRoute>()).Returns(testRoutes);
        var result = (ViewResult) controller.AddRoute(routeAdditionModel.LoopId);
        Assert.Equivalent(routeAdditionModel, result.Model);
    }

    [Fact]
    public async void LoopManagerController_AddRoute_ReturnPageOnModelError()
    {
        controller.ModelState.AddModelError(string.Empty, "Ha");
        var result = (ViewResult) await controller.AddRoute(routeAdditionModel);
        Assert.Equal(routeAdditionModel, result.Model);
    }

    #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    [Fact]
    public async void LoopManagerController_AddRoute_ReturnPageOnNullRoute()
    {
        mockService.Setup(x => x.GetById<BusRoute>(routeAdditionModel.RouteId)).Returns<BusRoute>(null);
        var result = (ViewResult) await controller.AddRoute(routeAdditionModel);
        Assert.Equal(routeAdditionModel, result.Model);
    }
    #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    [Fact]
    public async void LoopManagerController_AddRoute_SuccessfullyAddsRoute()
    {
        mockService.Setup(x => x.GetById<BusRoute>(routeAdditionModel.RouteId)).Returns(testRoutes[0]);
        mockService.Setup(x => x.SaveChanges());
        var result = (RedirectToActionResult) await controller.AddRoute(routeAdditionModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void LoopManagerController_DeleteLoop_SuccessfullyReturnsPage()
    {
        var result = (ViewResult) controller.DeleteLoop(deletionModel.Id);
        Assert.Equivalent(deletionModel, result.Model);
    }

    [Fact]
    public async void LoopManagerController_DeleteLoop_ReturnOnModelError()
    {
        controller.ModelState.AddModelError(string.Empty, "yet another error :/");
        var result = (ViewResult) await controller.DeleteLoop(deletionModel);
        Assert.Equal(deletionModel, result.Model);
    }

    [Fact]
    public async void LoopManagerController_DeleteLoop_SuccessfullyDeletesLoop()
    {
        mockService.Setup(x => x.DeleteById<Loop>(deletionModel.Id));
        var result = (RedirectToActionResult) await controller.DeleteLoop(deletionModel);
        Assert.Equal(HomeAction, result.ActionName);
    }
}
