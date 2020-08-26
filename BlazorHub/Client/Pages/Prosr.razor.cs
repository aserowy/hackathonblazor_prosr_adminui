using BlazorHub.Client.Business;
using Microsoft.AspNetCore.Components;
using prosr.Parser;
using prosr.Parser.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorHub.Client.Pages
{
    public partial class Prosr
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IBuilder Builder { get; set; }
        [Inject] public IAstStore AstStore { get; set; }
        [Inject] public IGenericHubClientFactory ClientFactory { get; set; }

        public Ast Ast { get; set; }
        public IGenericHubClient HubClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var prosr = await HttpClient.GetStringAsync("hub/helloworldhub/prosr");
            var ast = Builder.Build(prosr);

            AstStore.Store(ast);

            Ast = ast;
            HubClient = ClientFactory.Create(new Uri("https://localhost:5001/hub/helloworldhub"));
        }
    }
}
