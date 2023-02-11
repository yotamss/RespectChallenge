using Blazored.LocalStorage;

namespace Respect.Client.Authentication;

public class LocalStorageTokenStore : ITokenStore
{
    private const string Key = "Token";
    
    private readonly ISyncLocalStorageService localStorage;

    public LocalStorageTokenStore(ISyncLocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    public string GetToken()
    {
        return localStorage.GetItem<string>(Key);
    }

    public void SetToken(string? token)
    {
        localStorage.SetItem(Key, token);
    }
}