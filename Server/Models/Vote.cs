namespace Respect.Server.Models;

public class Vote : Entity
{
    public Petition Petition { get;}
    public string FirstName { get; }
    public string LastName { get; }
    public string Phone { get;}

    public Vote()
    {
    }

    public Vote(Petition petition, string firstName, string lastName, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Petition = petition;
    }
}