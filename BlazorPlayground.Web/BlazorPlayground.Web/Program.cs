using BlazorPlayground.Web.Client;
using BlazorPlayground.Web.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpLogging(o => { });

builder.Services.AddStateProvider();
builder.Services.AddSingleton<ApplicationState>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
}
else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorPlayground.Web.Client._Imports).Assembly);

app.Run();
