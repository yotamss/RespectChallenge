using Respect.Server.Data;
using Respect.Server.Models;

namespace Respect.Server.Logic;

public class SmsVerificationCodeSender : IVerificationCodeSender
{
    private readonly ILogger<SmsVerificationCodeSender> logger;
    private readonly PetitionContext petitionContext;

    public SmsVerificationCodeSender(ILogger<SmsVerificationCodeSender> logger, PetitionContext petitionContext)
    {
        this.logger = logger;
        this.petitionContext = petitionContext;
    }
    public async Task SendAsync(string verificationCode, string phone)
    {
        logger.LogInformation($"Asked to send sms to phone={phone} with verification code {verificationCode}");
        await petitionContext.SmsSet.AddAsync(new Sms(phone, $"Your verification code is: {verificationCode}"));
        await petitionContext.SaveChangesAsync();
    }
}