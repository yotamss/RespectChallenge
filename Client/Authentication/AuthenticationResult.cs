namespace Respect.Client.Authentication;

public record AuthenticationResult(bool Succeeded, IReadOnlyList<string> Errors);