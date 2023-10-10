using Blazored.LocalStorage;
using Example;
using Example.Options;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using pocketbase_csharp_sdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Make the loaded config available via dependency injection
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


var pbConfigurationSection = builder.Configuration.GetSection(PocketBaseOptions.Position);

builder.Services.Configure<PocketBaseOptions>(pbConfigurationSection);

var pbOptions = pbConfigurationSection.Get<PocketBaseOptions>();

builder.Services.AddSingleton<PocketBase>(sp =>
{
    return new PocketBase(pbOptions.BaseUrl);
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

//Authentication
builder.Services.AddScoped<AuthenticationStateProvider, PocketBaseAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
