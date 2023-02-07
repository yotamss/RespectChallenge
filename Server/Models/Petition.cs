using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Respect.Server.Models;

public class Petition: Entity
{
    private const int MinimumTargetVotes = 5;
    
    public string OwnerId { get; private set; }  // Of type ApplicationUser.Id
    
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int TargetVotes { get; private set; }
    
    // [ForeignKey("PetitionId")]
    public ICollection<Vote> Votes { get; private set; }
    
    // [ForeignKey("PetitionId")]
    // private ICollection<VoteForm> VoteForms { get; set; }

    public Petition()
    {
        // for ef core
    }
    
    public Petition(ApplicationUser owner, string name, string description, int targetVotes)
    {
        if (targetVotes < MinimumTargetVotes)
            throw new TargetVotesTooSmall($"Target votes is {targetVotes} but it must be at least {MinimumTargetVotes}.");
        
        OwnerId = owner.Id;
        Name = name;
        Description = description;
        TargetVotes = targetVotes;
        Votes = new List<Vote>();
    }
}

class TargetVotesTooSmall : Exception
{
    public TargetVotesTooSmall()
    {
    }

    protected TargetVotesTooSmall(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public TargetVotesTooSmall(string? message) : base(message)
    {
    }

    public TargetVotesTooSmall(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}