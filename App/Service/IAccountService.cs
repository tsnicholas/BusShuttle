using Microsoft.AspNetCore.Identity;
namespace App.Service;

public interface IAccountService
{
    Task<IdentityResult> CreateDriverAccount(string email, string password);
    Task UpdateAccountActivation(string email, bool IsActivated);
    Task<string> GetCurrentEmail(HttpContext context);
}
