using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Respect.Server.Models;

namespace Respect.Server.Data;

public class AuthenticationContext : IdentityDbContext<ApplicationUser>
{
    public AuthenticationContext(DbContextOptions<AuthenticationContext> config) : base(config)
    {
    }
}
