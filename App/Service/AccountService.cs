using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace App.Service;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;

    public AccountService(UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
    }

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

    public async Task<string> GetCurrentEmail(ClaimsPrincipal securityInformation) {
        var user = await _userManager.GetUserAsync(securityInformation) 
            ?? throw new Exception("Can't find User using Claims Principal.");
        return await _userManager.GetEmailAsync(user) ?? throw new Exception("Can't find current user's email.");
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>) _userStore;
    }
}
