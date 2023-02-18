using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Respect.Server.Data;
using Respect.Server.Logic;
using Respect.Server.Models;

namespace Respect.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class VoteFormController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly PetitionContext petitionContext;
    private readonly VoteFormManager voteFormManager;

    public VoteFormController(UserManager<ApplicationUser> userManager, PetitionContext petitionContext,
        VoteFormManager voteFormManager)
    {
        this.userManager = userManager;
        this.petitionContext = petitionContext;
        this.voteFormManager = voteFormManager;
    }

    [HttpPost]
    public async Task<ActionResult<Petition>> CreateVoteFormAsync(CreateVoteFormRequest voteFormRequest)
    {
        var petition = await petitionContext.Petitions.FindAsync(voteFormRequest.PetitionId);

        if (petition is null)
            return NotFound();

        var voteForm = voteFormManager.CreateVoteForm(petition, voteFormRequest.FirstName, voteFormRequest.LastName);
        
        await petitionContext.VoteForms.AddAsync(voteForm);
        await petitionContext.SaveChangesAsync();

        return Ok(new {voteForm.Id});
    }

    [HttpPatch("{id}/phone")]
    public async Task<ActionResult> AddPhoneAsync(AddPhoneRequest voteFormRequest, string id)
    {
        var voteForm = await petitionContext.VoteForms.FirstOrDefaultAsync(vf => vf.Id == id);

        if (voteForm is null)
            return NotFound();

        try
        {
            voteFormManager.SetPhone(voteForm, voteFormRequest.Phone);
        }
        catch (VoteFormLogicalError e)
        {
            return UnprocessableEntity(new UserReflectedErrorResponse(e.Message));
        }

        await petitionContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{id}/requestSms")]
    public async Task<ActionResult> SendVerificationAsync(string id)
    {
        var voteForm = await petitionContext.VoteForms.FindAsync(id); // maybe guid.parse is needed

        if (voteForm is null)
            return NotFound();
        try
        {
            await voteFormManager.SendVerificationAsync(voteForm);
        }
        catch (VoteFormLogicalError e)
        {
            return UnprocessableEntity(new UserReflectedErrorResponse(e.Message));
        }

        return Ok();
    }

    [HttpPatch("{id}/verify")]
    public async Task<ActionResult> VerifyAsync(VerifyCodeRequest verifyCodeRequest, string id)
    {
        var voteForm = await petitionContext.VoteForms
            .Include(vf => vf.Petition)
            .FirstOrDefaultAsync(vf => vf.Id == id); // maybe guid.parse is needed

        if (voteForm is null)
            return NotFound();

        if (!voteFormManager.IsVerificationCodeValid(voteForm, verifyCodeRequest.VerificationCode))
            return UnprocessableEntity(new UserReflectedErrorResponse("Incorrect verification code!"));

        try
        {
            var vote = voteFormManager.CreateVote(voteForm, verifyCodeRequest.VerificationCode);
            await petitionContext.Votes.AddAsync(vote);
            await petitionContext.SaveChangesAsync();
        }
        catch (VoteFormLogicalError e)
        {
            return UnprocessableEntity(new UserReflectedErrorResponse(e.Message));
        }

        return Ok();
    }


    public record CreateVoteFormRequest(string FirstName, string LastName, int PetitionId);

    public record AddPhoneRequest(string Phone);

    public record VerifyCodeRequest(string VerificationCode);
}