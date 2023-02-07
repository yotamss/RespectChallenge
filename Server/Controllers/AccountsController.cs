using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Respect.Server.Models;

namespace Respect.Server.Controllers;

/// <summary>
/// Manages user accounts
/// </summary>
[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;

    public AccountsController(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="request">The registration request</param>
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
    {
        var user = new ApplicationUser { Email = request.Email, UserName = Guid.NewGuid().ToString()};
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
            return Ok();

        return BadRequest(result.Errors);
    }

    public record RegistrationRequest(string Email, string Password);
}