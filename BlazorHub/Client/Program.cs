using BlazorHub.Client.Business;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using prosr.Parser;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorHub.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddLogging(bldr => bldr.SetMinimumLevel(LogLevel.Information));

            builder.Services.AddTransient<IActionTypeNameResolver, ActionTypeResolver>();
            builder.Services.AddTransient<IBuilder, Builder>();
            builder.Services.AddTransient<IFieldTypeResolver, FieldTypeResolver>();
            builder.Services.AddTransient<IGenericHubClientFactory, GenericHubClientFactory>();
            builder.Services.AddTransient<IMessageResolver, MessageResolver>();
            builder.Services.AddTransient<IMessageResolverFactory, MessageResolverFactory>();
            builder.Services.AddTransient<IMessageTypeFactory, MessageTypeFactory>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });

            await builder.Build().RunAsync();
        }
    }
}
