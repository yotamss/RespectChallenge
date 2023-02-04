using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Respect.Client;
using Respect.Client.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Respect.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Respect.ServerAPI"));

builder.Services.AddMudServices();
builder.Services.AddTransient<Authenticator>();
builder.Services.AddTransient<AccountRegistrar>();
builder.Services.AddHttpClient("serverHttpClient", (serviceProvider, client) =>
{
    client.BaseAddress = new Uri("http://localhost:7355");
});

builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("serverHttpClient"));

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<ITokenStore, LocalStorageTokenStore>();
builder.Services.AddTransient<AuthManager>();

await builder.Build().RunAsync();
