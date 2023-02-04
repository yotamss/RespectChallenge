using Blazored.LocalStorage;

namespace Respect.Client.Authentication;

public class LocalStorageTokenStore : ITokenStore
{
    private const string Key = "Token";
    
    private readonly ILocalStorageService localStorage;

    public LocalStorageTokenStore(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await localStorage.GetItemAsync<string>(Key);
    }

    public async Task SetTokenAsync(string? token)
    {
        await localStorage.SetItemAsync(Key, token);
    }
}