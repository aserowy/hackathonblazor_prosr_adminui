using prosr.Hub;
using System.Threading.Tasks;

namespace BlazorHub.Server.Hubs
{
    public class HelloWorldHub : HelloWorlHubBase
    {
        protected override Task<HelloResponse> HandleGreetAllAsync(HelloRequest message)
        {
            return Task.FromResult(new HelloResponse { ServerMessage = "Hello all.", Request = message });
        }

        protected override async Task<HelloResponse> HandleGreetCallerAsync(HelloRequest message)
        {
            await SendGreetCallOnAllAsync(message).ConfigureAwait(false);

            return new HelloResponse { ServerMessage = "Hello caller.", Request = message };
        }
    }
}
