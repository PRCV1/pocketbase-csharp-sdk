using Example;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using pocketbase_csharp_sdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<PocketBase>(sp =>
{
    return new PocketBase("https://sdk-todo-example.pockethost.io/");
});

builder.Services.AddMudServices();

await builder.Build().RunAsync();
