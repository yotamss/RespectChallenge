using System.Net.Http.Json;
using Respect.Client.DTO;

namespace Respect.Client.Accessors;

public class PetitionsManager
{
    private readonly HttpClient httpClient;

    public PetitionsManager(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<HttpResponseMessage> CreatePetitionAsync(CreatePetitionRequest petitionRequest)
    {
        return await httpClient.PostAsJsonAsync("/Petitions", petitionRequest);
    }
    
    public async Task<ICollection<PetitionDto>> ListAllPetitionsAsync()
    {
        return await httpClient.GetFromJsonAsync<ICollection<PetitionDto>>("/Petitions");
    }
    
    public async Task<PetitionDto?> GetPetitionAsync(int petitionId)
    {
        return await httpClient.GetFromJsonAsync<PetitionDto?>($"/Petitions/{petitionId}");
    }
    
    public async Task<FlagResponse?> GetFlagAsync(int petitionId)
    {
        return await httpClient.GetFromJsonAsync<FlagResponse>($"/Petitions/{petitionId}/flag");
    }
}

public record FlagResponse(string Message);

public record CreatePetitionRequest(string Name, string Description, int TargetVotes);