using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Respect.Server.Models;

namespace Respect.Server.Data;

public class PetitionContext : DbContext
{
    public PetitionContext(DbContextOptions<PetitionContext> config) : base(config)
    {
    }
    public DbSet<Petition> Petitions { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<VoteForm> VoteForms { get; set; }
    public DbSet<Sms> SmsSet { get; set; }
}
