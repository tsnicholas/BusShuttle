using Moq;
using App.Service;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
namespace App.Tests.Service;

public class AccountServiceTests
{
    #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    private static readonly IUserStore<IdentityUser> mockUserStore = new MockUserEmailStore<IdentityUser>();
    private static readonly Mock<UserManager<IdentityUser>> mockUserManager = 
        new(mockUserStore, null, null, null, null, null, null, null, null);
    private readonly AccountService service;
    #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    public AccountServiceTests()
    {
        service = new AccountService(mockUserManager.Object, mockUserStore);
    }

    [Fact]
    public async void AccountService_CreateDriverAccount_Successfully()
    {
        const string driverEmail = "Dummy2377@outlook.com";
        const string driverPassword = "Clever Password";
        Claim expectedClaim = new("activated", "True");
        mockUserManager.Setup(x => x.AddClaimAsync(It.IsAny<IdentityUser>(), expectedClaim))
            .Returns(Task.FromResult(IdentityResult.Success));
        mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Driver"))
            .Returns(Task.FromResult(IdentityResult.Success));
        mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), driverPassword))
            .Returns(Task.FromResult(IdentityResult.Success));
        await service.CreateDriverAccount(driverEmail, driverPassword);
    }

    [Theory]
    [InlineData([false])]
    [InlineData([true])]
    public async void AccountService_UpdateAccountActivation_Successfully(bool result)
    {
        const string driverEmail = "tsnicholas@bsu.edu";
        IdentityUser user = Activator.CreateInstance<IdentityUser>();
        mockUserManager.Setup(x => x.FindByEmailAsync(driverEmail)).Returns(Task.FromResult(user));
        IList<Claim> userClaims = [new Claim("activated", "False")];
        mockUserManager.Setup(x => x.GetClaimsAsync(user)).Returns(Task.FromResult(userClaims));
        mockUserManager.Setup(x => x.ReplaceClaimAsync(user, userClaims[0], new("activated", result.ToString())))
            .Returns(Task.FromResult(IdentityResult.Success));
        await service.UpdateAccountActivation(driverEmail, result);
    }

    [Fact]
    public async void AccountService_GetCurrentEmail_Successfully()
    {
        const string mockEmail = "Your Email"; 
        IdentityUser? user = Activator.CreateInstance<IdentityUser>();
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal());
        mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(user));
        mockUserManager.Setup(x => x.GetEmailAsync(user)).Returns(Task.FromResult(mockEmail));
        string result = await service.GetCurrentEmail(mockHttpContext.Object);
        Assert.Equal(mockEmail, result);
    }
}
