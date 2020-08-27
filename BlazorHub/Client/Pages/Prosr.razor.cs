using BlazorHub.Client.Business;
using Microsoft.AspNetCore.Components;
using prosr.Parser;
using prosr.Parser.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorHub.Client.Pages
{
    public partial class Prosr : IDisposable
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IBuilder Builder { get; set; }
        [Inject] public IAstStore AstStore { get; set; }
        [Inject] public IGenericHubClientFactory ClientFactory { get; set; }

        public Ast Ast { get; set; }
        public IGenericHubClient HubClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
#if DEBUG
            await Task.Delay(10000);
#endif
            if (Ast is null)
            {

                var prosr = await HttpClient.GetStringAsync("hub/helloworldhub/prosr");
                var ast = Builder.Build(prosr);

                AstStore.Store(ast);

                Ast = ast;
            }

            if (HubClient is null)
            {
                HubClient = ClientFactory.Create(new Uri("https://localhost:5001/hub/helloworldhub"));
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            HubClient?.InitializeHubConnection();
        }

        public void Dispose()
        {
            HubClient.Dispose();
        }
    }
}
