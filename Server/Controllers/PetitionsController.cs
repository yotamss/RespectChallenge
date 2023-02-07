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
[Authorize]
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

    [HttpPost]
    public async Task<IActionResult> CreatePetitionAsync(CreatePetitionRequest request)
    {
        var currentUser = await userManager.FindByEmailAsync(HttpContext.User.Identity!.Name!);
        
        if (currentUser is null)
            return BadRequest("Could not find user.");
        
        try
        {
            var petition = new Petition(currentUser, request.Name, request.Description, request.targetVotes);
            await petitionContext.Petitions.AddAsync(petition);
            await petitionContext.SaveChangesAsync();
        }
        catch (TargetVotesTooSmall e)
        {
            return UnprocessableEntity(new UserReflectedErrorResponse(e.Message));
        }

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Petition>> GetPetitionAsync(int id)
    {
        var petition = await petitionContext.Petitions
            .Include(p => p.Votes)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (petition is not null)
            return Ok(petition);

        return NotFound();
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