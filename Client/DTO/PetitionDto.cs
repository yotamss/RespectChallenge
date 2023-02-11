namespace Respect.Client.DTO;

public record PetitionDto(int Id, string OwnerId, string OwnerEmail, string Name, string Description, int TargetVotes,
    ICollection<VoteDto> Votes);