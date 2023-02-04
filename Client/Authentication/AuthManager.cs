using Blazored.LocalStorage;

namespace Respect.Client.Authentication;

public class AuthManager
{
    private readonly ITokenStore tokenStore;

    public AuthManager(ITokenStore tokenStore)
    {
        this.tokenStore = tokenStore;
    }

    public async Task SetTokenAsync(string? token)
    {
        await tokenStore.SetTokenAsync(token);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await tokenStore.GetTokenAsync();
        return token is not null;
    }

    public async Task LogoutAsync()
    {
        await tokenStore.SetTokenAsync(null);
    }

}