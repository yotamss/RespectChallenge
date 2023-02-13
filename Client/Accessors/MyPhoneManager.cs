using System.Net.Http.Json;
using Respect.Client.DTO;

namespace Respect.Client.Accessors;

public class MyPhoneManager
{
    private readonly HttpClient httpClient;

    public MyPhoneManager(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<Sms>> GetAllSmsSent()
    {
        // return (await httpClient.GetFromJsonAsync<IEnumerable<Sms>>($"/MyPhoneEmulator")) ?? new List<Sms>();
        return await httpClient.GetFromJsonAsync<IEnumerable<Sms>>($"/MyPhoneEmulator");
    }
}

public record Sms(string PhoneNumber, string Text);
