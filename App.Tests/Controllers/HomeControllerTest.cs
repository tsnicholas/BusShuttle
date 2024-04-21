using Moq;
using App.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
namespace App.Test.Controllers;

public class HomeControllerTest
{
    private static Mock<ClaimsPrincipal> mockPrincipal = new();
    private readonly HomeController controller;

    public HomeControllerTest()
    {
        ControllerContext context = InitializeTestContext();
        controller = new HomeController() {
            ControllerContext = context,
        };
    }

    private static ControllerContext InitializeTestContext()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(m => m.User).Returns(mockPrincipal.Object);
        return new ControllerContext() { 
            HttpContext = mockHttpContext.Object 
        };
    }

    [Fact]
    public void HomeController_Index_ReturnsManagerScreenToManagers()
    {
        mockPrincipal.Setup(m => m.IsInRole("Manager")).Returns(true);
        var result = (RedirectToActionResult) controller.Index();
        Assert.Equal("Manager", result.ActionName);
    }

    [Fact]
    public void HomeController_Index_ReturnsDriverScreenToNonManagers()
    {
        mockPrincipal.Setup(m => m.IsInRole("Manager")).Returns(false);
        var result = (RedirectToActionResult) controller.Index();
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Driver", result.ControllerName);
    }
}
