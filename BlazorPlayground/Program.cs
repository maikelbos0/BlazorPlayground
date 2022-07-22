using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorPlayground {

    /*
     * TODO
     * Snap to existing point (how? point != anchor)
     * Add stroke array?
     * Add opacity
     * Add fill opacity?
     * Add stroke opacity?
     * Add arc
     * Add cubic bezier curve
     * Add save/export of svg
     * Add load/import of svg
     */

    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
