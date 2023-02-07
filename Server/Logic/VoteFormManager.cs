using System.Security.Cryptography;
using Respect.Server.Data;
using Respect.Server.Models;

namespace Respect.Server.Logic;

public class VoteFormManager
{
    private readonly PetitionContext petitionContext;
    private readonly IVerificationCodeSender verificationCodeSender;
    private readonly ILogger<VoteFormManager> logger;

    public VoteFormManager(PetitionContext petitionContext, IVerificationCodeSender verificationCodeSender, ILogger<VoteFormManager> logger)
    {
        this.petitionContext = petitionContext;
        this.verificationCodeSender = verificationCodeSender;
        this.logger = logger;
    }

    public VoteForm CreateVoteForm(Petition petition, string firstName, string lastName)
    {
        return new VoteForm(petition, firstName, lastName);
    }

    public void SetPhone(VoteForm voteForm, string phone)
    {
        if (voteForm.State != VoteFormState.AwaitingPhoneNumber)
            throw new VoteFormLogicalError("Phone number already set for this vote form");
    
        if (petitionContext.Votes.Any(v => v.Phone == phone))
            throw new VoteFormLogicalError("A person already voted with this phone number");

        phone = NormalizePhone(phone);
        
        voteForm.Phone = phone;
        voteForm.State = VoteFormState.AwaitingVerification;
    }

    private string NormalizePhone(string phone)
    {
        // Remove everything but numbers from the phone
        var onlyDigits = phone.Where(char.IsDigit).ToArray();
        return new string(onlyDigits);
    }

    public async Task SendVerificationAsync(VoteForm voteForm)
    {
        if (voteForm.State != VoteFormState.AwaitingVerification)
            throw new VoteFormLogicalError("State is incorrect, could not send verification code");
        
        voteForm.VerificationCode = GenerateVerificationCode();
        await petitionContext.SaveChangesAsync();
        logger.LogInformation($"Generated new verification code: {voteForm.VerificationCode}");
        
        await verificationCodeSender.SendAsync(voteForm.VerificationCode, voteForm.Phone);
    }

    private string GenerateVerificationCode()
    {
        return RandomNumberGenerator.GetInt32(100_000, 1_000_000).ToString();
    }

    public bool IsVerificationCodeValid(VoteForm voteForm, string code)
    {
        return voteForm.VerificationCode == code;
    }

    public Vote CreateVote(VoteForm voteForm, string verificationCode)
    {
        // Rate limiting exists to prevent BF on the verification code
        if (voteForm.State != VoteFormState.AwaitingVerification)
            throw new VoteFormLogicalError("Phone not verified yet");

        if (verificationCode != voteForm.VerificationCode)
            throw new VoteFormLogicalError("Incorrect verification code!");

        voteForm.State = VoteFormState.VoteCreated;
        return new Vote(voteForm.Petition, voteForm.FirstName, voteForm.LastName, voteForm.Phone);
    }
}