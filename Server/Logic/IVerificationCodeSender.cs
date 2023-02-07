namespace Respect.Server.Logic;

public interface IVerificationCodeSender
{
    public Task SendAsync(string verificationCode, string phone);
}