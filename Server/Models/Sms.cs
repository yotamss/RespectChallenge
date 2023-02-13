namespace Respect.Server.Models;

public class Sms : Entity
{
    public string PhoneNumber { get; private set; }
    public string Text { get; private set; }

    public Sms()
    {
    }

    public Sms(string phoneNumber, string text)
    {
        PhoneNumber = phoneNumber;
        Text = text;
    }
}