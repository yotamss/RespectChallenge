using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Respect.Server.Models;

public enum VoteFormState
{
    AwaitingPhoneNumber,
    AwaitingVerification,
    VoteCreated,
}

public class VoteForm : Entity
{
    [Key] public new string Id { get; private set; }
    public string? VerificationCode { get; set; }
    public int PetitionId { get; set; }
    public Petition Petition { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public VoteFormState State { get; set; }
    public string? Phone { get; set; }
    
    public VoteForm()
    {
    }

    public VoteForm(Petition petition, string firstName, string lastName)
    {
        Id = Guid.NewGuid().ToString();
        FirstName = firstName;
        LastName = lastName;
        PetitionId = petition.Id;
        State = VoteFormState.AwaitingPhoneNumber;
    }
}

public class VoteFormLogicalError : Exception
{
    public VoteFormLogicalError()
    {
    }

    protected VoteFormLogicalError(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public VoteFormLogicalError(string? message) : base(message)
    {
    }

    public VoteFormLogicalError(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}