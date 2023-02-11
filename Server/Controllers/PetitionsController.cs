using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Respect.Server.Data;
using Respect.Server.Models;

namespace Respect.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PetitionsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly PetitionContext petitionContext;

    public PetitionsController(UserManager<ApplicationUser> userManager, PetitionContext petitionContext)
    {
        this.userManager = userManager;
        this.petitionContext = petitionContext;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Petition>> CreatePetitionAsync(CreatePetitionRequest request)
    {
        var currentUser = await userManager.FindByEmailAsync(HttpContext.User.Identity!.Name!);

        if (currentUser is null)
            return BadRequest("Could not find user.");

        try
        {
            var petition = new Petition(currentUser, request.Name, request.Description, request.targetVotes);
            await petitionContext.Petitions.AddAsync(petition);
            await petitionContext.SaveChangesAsync();
            return Ok(petition);
        }
        catch (TargetVotesTooSmall e)
        {
            return UnprocessableEntity(new UserReflectedErrorResponse(e.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Petition>> GetPetitionAsync(int id)
    {
        var petition = await petitionContext.Petitions
            .Include(p => p.Votes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (petition is null) return NotFound();

        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };
        var serialized = JsonSerializer.Serialize(petition, options);

        return Ok(serialized);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Petition>>> ListPetitionsAsync()
    {
        var petitions = await petitionContext.Petitions
            .Include(p => p.Votes)
            .AsNoTracking()
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        var serialized = JsonSerializer.Serialize(petitions, options);

        return Ok(serialized);
    }

    public record CreatePetitionRequest(string Name, string Description, int targetVotes);
}