using Blazored.LocalStorage;
using Example;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using pocketbase_csharp_sdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<PocketBase>(sp =>
{
    return new PocketBase("https://sdk-todo-example.pockethost.io/");
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

//Authentication
builder.Services.AddScoped<AuthenticationStateProvider, PocketBaseAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
