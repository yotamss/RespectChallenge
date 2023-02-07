namespace Respect.Server.Controllers;

public record UserReflectedErrorResponse(string[] Errors)
{
    public UserReflectedErrorResponse(string incorrectLoginInfo) : this(new[] { incorrectLoginInfo })
    {
    }
}