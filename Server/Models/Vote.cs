namespace Respect.Server.Models;

public class Vote : Entity
{
    public int PetitionId { get; private set; }
    public Petition Petition { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }

    public Vote()
    {
    }

    public Vote(Petition petition, string firstName, string lastName, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        PetitionId = petition.Id;
    }
}