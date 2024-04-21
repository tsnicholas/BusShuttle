using App.Controllers;
using App.Models.Manager;
using BusShuttleModel;
using Database.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace App.Tests.Controllers;

public class BusManagerControllerTests
{
    private static readonly string HomeAction = "Index";
    private static readonly List<Bus> testBuses = [ new(1, 69), new(2, 42) ];
    private static readonly Mock<IDatabaseService> mockService = new();
    private readonly BusManagerController controller;

    public BusManagerControllerTests()
    {
        controller = new BusManagerController(mockService.Object);
        mockService.Setup(x => x.GetAll<Bus>()).Returns(testBuses);
        mockService.Setup(x => x.GenerateId<Bus>()).Returns(testBuses.Count + 1);
    }

    [Fact]
    public void BusManagerController_Index_ReturnPageSuccessfully()
    {
        IEnumerable<BusViewModel> viewModels = [
            BusViewModel.FromBus(testBuses[0]), BusViewModel.FromBus(testBuses[1])
        ];
        var result = (ViewResult) controller.Index();
        Assert.Equivalent(viewModels, result.Model);
    }

    [Fact]
    public void BusManagerController_CreateBus_ReturnCreationView()
    {
        CreateBusModel creationModel = CreateBusModel.CreateBus(testBuses.Count + 1);
        var result = (ViewResult) controller.CreateBus();
        Assert.Equivalent(creationModel, result.Model);
    }

    [Fact]
    public async void BusManagerController_CreateBus_CreatesBusSuccessfully()
    {
        Bus newBus = new(3, 89);
        CreateBusModel creationModel = new()
        {
            Id = newBus.Id, BusNumber = newBus.BusNumber
        };
        mockService.Setup(x => x.CreateEntity(newBus));
        var result = (RedirectToActionResult) await controller.CreateBus(creationModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void BusManagerController_EditBus_ReturnEditView()
    {
        Bus selectedBus = testBuses[0];
        mockService.Setup(x => x.GetById<Bus>(selectedBus.Id)).Returns(selectedBus);
        EditBusModel expectedModel = EditBusModel.FromBus(selectedBus);
        var result = (ViewResult) controller.EditBus(selectedBus.Id);
        Assert.Equivalent(expectedModel, result.Model);
    }

    [Fact]
    public async void BusManagerController_EditBus_UpdatesBusSuccessfully()
    {
        const int updatedBusNumber = 0;
        EditBusModel editModel = EditBusModel.FromBus(testBuses[0]);
        editModel.BusNumber = updatedBusNumber;
        mockService.Setup(x => x.UpdateById(editModel.Id, new Bus(editModel.Id, updatedBusNumber)));
        var result = (RedirectToActionResult) await controller.EditBus(editModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void BusManagerController_DeleteBus_ReturnDeleteView()
    {
        Bus selectedBus = testBuses[1];
        mockService.Setup(x => x.GetById<Bus>(selectedBus.Id)).Returns(selectedBus);
        DeleteBusModel expectedModel = DeleteBusModel.DeleteBus(selectedBus.Id);
        var result = (ViewResult) controller.DeleteBus(selectedBus.Id);
        Assert.Equivalent(expectedModel, result.Model);
    }

    [Fact]
    public async void BusManagerController_DeleteBus_DeleteBusSuccessfully()
    {
        DeleteBusModel deleteModel = new()
        {
            Id = testBuses[1].Id
        };
        mockService.Setup(x => x.DeleteById<Bus>(deleteModel.Id));
        var result = (RedirectToActionResult) await controller.DeleteBus(deleteModel);
        Assert.Equal(HomeAction, result.ActionName);
    }
}
