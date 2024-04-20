using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
namespace App.Service;

public class AccountService(
    UserManager<IdentityUser> userManager,
    IUserStore<IdentityUser> userStore,
    IUserEmailStore<IdentityUser> emailStore
    ) : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly IUserStore<IdentityUser> _userStore = userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore = emailStore;

    public async Task<IdentityResult> CreateDriverAccount(string email, string password)
    {
        var user = CreateUser();
        var claim = new Claim("activated", "True");
        await _userManager.AddToRoleAsync(user, "Driver");
        await _userManager.AddClaimAsync(user, claim);
        await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
        return await _userManager.CreateAsync(user, password);
    }

    private IdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<IdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    public async Task UpdateAccountActivation(string email, bool IsActivated)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null) return;
        IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
        var previousClaim = userClaims.Single(claim => claim.Type == "activated");
        var newClaim = new Claim("activated", IsActivated.ToString());
        await _userManager.ReplaceClaimAsync(user, previousClaim, newClaim);
    }

    public async Task<string> GetCurrentEmail() {
        var securityInformation = GetSecurityInformation();
        var user = await _userManager.GetUserAsync(securityInformation) 
            ?? throw new Exception("Can't find User using Claims Principal.");
        return await _userManager.GetEmailAsync(user) ?? throw new Exception("Can't find current user's email.");
    }

    private static ClaimsPrincipal GetSecurityInformation()
    {
        Exception exception = new("Can't verify user's identity");
        if(Thread.CurrentPrincipal == null) throw exception;
        IIdentity identity = Thread.CurrentPrincipal.Identity ?? throw exception;
        return new(identity);
    }
}
