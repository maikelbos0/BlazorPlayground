using BlazorPlayground.Web.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddStateProvider();
builder.Services.AddSingleton<ApplicationState>();

await builder.Build().RunAsync();
