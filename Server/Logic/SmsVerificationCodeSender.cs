namespace Respect.Server.Logic;

public class SmsVerificationCodeSender : IVerificationCodeSender
{
    private readonly ILogger<SmsVerificationCodeSender> logger;

    public SmsVerificationCodeSender(ILogger<SmsVerificationCodeSender> logger)
    {
        this.logger = logger;
    }
    public async Task SendAsync(string verificationCode, string phone)
    {
        logger.LogInformation($"Asked to send sms to phone={phone} with verification code {verificationCode}");
        await Task.Delay(1);
    }
}