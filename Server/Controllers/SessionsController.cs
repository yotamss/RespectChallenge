using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Respect.Server.Models;

namespace Respect.Server.Controllers;

/// <summary>
/// Manages user sessions (logins)
/// </summary>
[ApiController]
[Route("[controller]")]
public class SessionsController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IConfiguration configuration;

    public SessionsController(SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        this.signInManager = signInManager;
        this.configuration = configuration;
    }

    /// <summary>
    /// Logs a user in.
    /// </summary>
    /// <response code="200">Returns the access token</response>
    /// <response code="400">If the credentials are invalid</response>
    [HttpPost]
    public async Task<ActionResult<AuthenticationResult>> LogInAsync([FromBody] AuthenticationCredentials credentials)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(credentials.Email);
        
        if (user is null) return BadRequest();
        
        var result = await signInManager.PasswordSignInAsync(user, credentials.Password, false, false);
        
        if (!result.Succeeded) return BadRequest();
        
        var token = GenerateAccessToken(user);
        return new AuthenticationResult(token);

    }

    private string GenerateAccessToken(ApplicationUser user)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Authentication:SecretKey"]);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtHandler.CreateToken(tokenDescriptor);

        return jwtHandler.WriteToken(token);
    }

    public record AuthenticationCredentials(string Email, string Password);

    public record AuthenticationResult(string AccessToken);
}