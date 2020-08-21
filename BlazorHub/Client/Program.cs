using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using prosr.Parser;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorHub.Client.Tmp;

namespace BlazorHub.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient<IActionTypeNameResolver, ActionTypeResolver>();
            builder.Services.AddTransient<IMessageResolver, MessageResolver>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });
            builder.Services.AddTransient<IBuilder, Builder>();

            await builder.Build().RunAsync();
        }
    }
}
