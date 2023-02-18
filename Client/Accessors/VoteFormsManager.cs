using System.Net.Http.Json;
using Respect.Client.DTO;

namespace Respect.Client.Accessors;

public class VoteFormsManager
{
    private readonly HttpClient httpClient;

    public VoteFormsManager(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> CreateVoteFormAsync(CreateVoteFormRequest voteFormRequest)
    {
        return await httpClient.PostAsJsonAsync("/VoteForm", voteFormRequest);
    }

    public async Task<HttpResponseMessage> AddPhoneAsync(AddPhoneRequest addPhoneRequest, string voteFormId)
    {
        return await httpClient.PatchAsJsonAsync($"/VoteForm/{voteFormId}/phone", addPhoneRequest);
    }

    public async Task<HttpResponseMessage> AskForVerificationCodeAsync(string voteFormId)
    {
        return await httpClient.PostAsJsonAsync(
            $"/VoteForm/{voteFormId}/requestSms",
            new AskForVerificationCode()
        );
    }

    public async Task<HttpResponseMessage> VerifyCodeRequestAsync(VerifyCodeRequest verifyCodeRequest, string voteFormId)
    {
        return await httpClient.PatchAsJsonAsync($"/VoteForm/{voteFormId}/verify", verifyCodeRequest);
    }
}

public record CreateVoteFormRequest(string FirstName, string LastName, int PetitionId);

public record AddPhoneRequest(string Phone);
public record AskForVerificationCode;
public record VerifyCodeRequest(string VerificationCode);
