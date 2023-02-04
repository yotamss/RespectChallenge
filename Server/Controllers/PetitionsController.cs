using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    
    // [HttpPost]
    // public async Task<IActionResult> CreatePetitionAsync(CreatePetitionRequest request)
    // {
    //     // var currentUser = userManager.get
    //     // var petition = new Petition(this.User., request.Name, request.Description);
    // }

    public record CreatePetitionRequest(string Name, string Description);

}