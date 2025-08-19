using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddStateProvider();
builder.Services.AddStateProvider2();

await builder.Build().RunAsync();
