using Microsoft.AspNetCore.Mvc;
using Moq;
using App.Controllers;
using App.Service;
using BusShuttleModel;
using Database.Service;
using App.Models.Manager;
using Microsoft.AspNetCore.Identity;
namespace App.Tests.Controllers;

public class DriverManagerControllerTest
{
    private static readonly string HomeAction = "Index";
    private static readonly List<Driver> testDrivers = [
        new(1, "Tim", "Nicholas", "tsnicholas@bsu.edu"),
        new(2, "Morgan", "Freeman", "MFreeman564@Gmail.com")
    ];
    private static readonly CreateDriverModel creationModel = new()
    {
        Id = 3, FirstName = "Bill", LastName = "Gates", Email = "Watergate@outlook.com", Password = "clever password"
    };
    private static readonly EditDriverModel editModel = new()
    {
        Id = testDrivers[0].Id,
        FirstName = testDrivers[0].FirstName, 
        LastName = testDrivers[0].LastName,
        Email = testDrivers[0].Email
    };
    private static readonly DeleteDriverModel deletionModel = DeleteDriverModel.DeleteDriver(testDrivers[1].Id);

    private static readonly Mock<IAccountService> mockAccountService = new();
    private static readonly Mock<IDatabaseService> mockDatabaseService = new();
    private readonly DriverManagerController controller;

    public DriverManagerControllerTest()
    {
        controller = new(mockAccountService.Object, mockDatabaseService.Object);
        mockDatabaseService.Setup(x => x.GetAll<Driver>()).Returns(testDrivers);
    }

    [Fact]
    public void DriverManagerController_Index_ReturnPageSuccessfully()
    {
        IEnumerable<DriverViewModel> expectedModels = [
            DriverViewModel.FromDriver(testDrivers[0]), 
            DriverViewModel.FromDriver(testDrivers[1])
        ];
        var result = (ViewResult) controller.Index();
        Assert.Equivalent(expectedModels, result.Model);
    }

    [Fact]
    public void DriverManagerController_CreateDriver_ReturnPageSuccessfully()
    {
        CreateDriverModel creationModel = CreateDriverModel.CreateDriver(testDrivers.Count + 1);
        var result = (ViewResult) controller.CreateDriver();
        Assert.Equivalent(creationModel, result.Model);
    }

    [Fact]
    public async void DriverManagerController_CreateDriver_InvalidModelReturnsPage()
    {
        controller.ModelState.AddModelError(string.Empty, "I am error.");
        var result = (ViewResult) await controller.CreateDriver(creationModel);
        Assert.Equivalent(creationModel, result.Model);
    }

    [Fact]
    public async void DriverManagerController_CreateDriver_IdentityResultErrorReturnsPage()
    {
        IdentityResult identityResult = IdentityResult.Failed(
            new IdentityError(){Code = "This is a code", Description = "Insert some error here."},
            new IdentityError(){Code = "404", Description = "Quoth the Server"}
        );
        mockAccountService.Setup(x => x.CreateDriverAccount(creationModel.Email, creationModel.Password))
            .Returns(Task.FromResult(identityResult));
        var viewResult = (ViewResult) await controller.CreateDriver(creationModel);
        Assert.Equivalent(creationModel, viewResult.Model);
    }

    [Fact]
    public async void DriverManagerController_CreateDriver_CreatesDriverSuccessfully()
    {
        mockAccountService.Setup(x => x.CreateDriverAccount(creationModel.Email, creationModel.Password))
            .Returns(Task.FromResult(IdentityResult.Success));
        Driver expectedDriver = new(creationModel.Id, creationModel.FirstName, creationModel.LastName, creationModel.Email);
        mockDatabaseService.Setup(x => x.CreateEntity(expectedDriver));
        var result = (RedirectToActionResult) await  controller.CreateDriver(creationModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void DriverManagerController_EditDriver_ReturnPageSuccessfully()
    {
        Driver selectedDriver = testDrivers[0];
        EditDriverModel editModel = EditDriverModel.FromDriver(selectedDriver);
        mockDatabaseService.Setup(x => x.GetById<Driver>(selectedDriver.Id)).Returns(selectedDriver);
        var result = (ViewResult) controller.EditDriver(selectedDriver.Id);
        Assert.Equivalent(editModel, result.Model);
    }

    [Fact]
    public async void DriverManagerController_EditDriver_ReturnPageOnModelError()
    {
        controller.ModelState.AddModelError(string.Empty, "This is an error.");
        var result = (ViewResult) await controller.EditDriver(editModel);
        Assert.Equivalent(editModel, result.Model);
    }

    [Fact]
    public async void DriverManagerController_EditDriver_UpdatesDriverSuccessfully()
    {
        Driver expectedDriver = new(editModel.Id, editModel.FirstName, editModel.LastName, editModel.Email);
        mockDatabaseService.Setup(x => x.UpdateById(expectedDriver.Id, expectedDriver));
        var result = (RedirectToActionResult) await controller.EditDriver(editModel);
        Assert.Equal(HomeAction, result.ActionName);
    }

    [Fact]
    public void DriverManagerController_DeleteDriver_ReturnPageSuccessfully()
    {
        var result = (ViewResult) controller.DeleteDriver(deletionModel.Id);
        Assert.Equivalent(deletionModel, result.Model);
    }

    [Fact]
    public async void DriverManagerController_DeleteDriver_ReturnPageOnModelError()
    {
        controller.ModelState.AddModelError(string.Empty, "I'm in your walls.");
        var result = (ViewResult) await controller.DeleteDriver(deletionModel);
        Assert.Equal(deletionModel, result.Model);
    }

    [Fact]
    public async void DriverManagerController_DeleteDriver_DeleteDriverSuccessfully()
    {
        mockDatabaseService.Setup(x => x.DeleteById<Driver>(deletionModel.Id));
        var result = (RedirectToActionResult) await controller.DeleteDriver(deletionModel);
        Assert.Equal(HomeAction, result.ActionName);
    }
}
