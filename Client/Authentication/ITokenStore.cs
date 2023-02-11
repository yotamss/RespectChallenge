namespace Respect.Client.Authentication;

public interface ITokenStore
{
    string GetToken();
    void SetToken(string? token);
}