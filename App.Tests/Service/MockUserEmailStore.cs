using Microsoft.AspNetCore.Identity;
namespace App.Tests.Service;

public class MockUserEmailStore<TUser> : IUserEmailStore<TUser> where TUser : class
{
    public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetEmailAsync(TUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailAsync(TUser user, string? email, CancellationToken cancellationToken)
    {
        if(email == null) throw new InvalidOperationException();
        return Task.CompletedTask;
    }

    public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task SetNormalizedEmailAsync(TUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        if(normalizedEmail == null) throw new InvalidOperationException();
        return Task.CompletedTask;
    }

    public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        if(normalizedName == null) throw new InvalidOperationException();
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken)
    {
        if(userName == null) throw new InvalidOperationException();
        return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}
