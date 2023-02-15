using Example;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using pocketbase_csharp_sdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<PocketBase>(sp =>
{
    return new PocketBase(builder.HostEnvironment.BaseAddress);
});

await builder.Build().RunAsync();
