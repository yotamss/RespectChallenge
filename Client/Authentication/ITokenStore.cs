namespace Respect.Client.Authentication;

public interface ITokenStore
{
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string? token);
}