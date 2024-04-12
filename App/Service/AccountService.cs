using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        IdentityUser user = await _userManager.FindByEmailAsync(email);
        IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
        var previousClaim = userClaims.Single(claim => claim.Type == "activated");
        var newClaim = new Claim("activated", IsActivated.ToString());
        await _userManager.ReplaceClaimAsync(user, previousClaim, newClaim);
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
