using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Respect.Client.Authentication;

public class AuthManager
{
    private readonly ITokenStore tokenStore;

    public AuthManager(ITokenStore tokenStore, HttpClient httpClient)
    {
        this.tokenStore = tokenStore;
    }

    public void SetToken(string? token)
    {
        tokenStore.SetToken(token);
    }

    public bool IsAuthenticated()
    {
        var token = tokenStore.GetToken();
        return token is not null;
    }

    public void Logout()
    {
        tokenStore.SetToken(null);
    }
}