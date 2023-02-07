using System.Collections.Immutable;
using System.Net.Http.Json;

namespace Respect.Client.Authentication;

public class AccountRegistrar
{
    private readonly HttpClient server;

    public AccountRegistrar(HttpClient server)
    {
        this.server = server;
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        using var response = await server.PostAsJsonAsync("/accounts", new
        {
            Email = email,
            Password = password
        }, cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            return new AuthenticationResult(Succeeded: true, Errors: ImmutableList<string>.Empty);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
        var errors = errorResponse!.Errors.ToImmutableList();
        return new AuthenticationResult(Succeeded: false, errors);
    }
}