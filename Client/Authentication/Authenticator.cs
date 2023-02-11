using System.Collections.Immutable;
using System.Net.Http.Json;

namespace Respect.Client.Authentication;

public class Authenticator
{
    private readonly HttpClient server;
    private readonly AuthManager authManager;

    public Authenticator(HttpClient server, AuthManager authManager)
    {
        this.server = server;
        this.authManager = authManager;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        using var response = await server.PostAsJsonAsync("/sessions", new
        {
            Email = email,
            Password = password
        }, cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<SuccessfulAuthenticationResponse>(
                cancellationToken: cancellationToken).ConfigureAwait(false);

            authManager.SetToken(responseContent!.AccessToken);
            
            return new AuthenticationResult(Succeeded: true, Errors: ImmutableList<string>.Empty);
        }

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
        var errors = errorResponse!.Errors.ToImmutableList();
        return new AuthenticationResult(Succeeded: false, errors);
    }

    private record SuccessfulAuthenticationResponse(string? AccessToken);
}