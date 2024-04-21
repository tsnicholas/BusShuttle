using Moq;
using App.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
namespace App.Test.Controllers;

public class HomeControllerTest
{
    private readonly HomeController controller;

    public HomeControllerTest()
    {
        controller = new HomeController();
        
    }

    [Fact]
    public void HomeController_Index_ReturnsManagerScreenToManagers()
    {
        var fakePrinciple = new Mock<IPrincipal>();
        fakePrinciple.Setup(e => e.IsInRole("Manager")).Returns(true);
        Thread.CurrentPrincipal = fakePrinciple.Object;
        var result = (RedirectToActionResult) controller.Index();
        Assert.Equal("Manager", result.ActionName);
    }

    [Fact]
    public void HomeController_Index_ReturnsDriverScreenToNonManagers()
    {
        var fakePrinciple = new Mock<IPrincipal>();
        fakePrinciple.Setup(e => e.IsInRole("Manager")).Returns(false);
        Thread.CurrentPrincipal = fakePrinciple.Object;
        var result = (RedirectToActionResult) controller.Index();
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Driver", result.ControllerName);
    }
}
