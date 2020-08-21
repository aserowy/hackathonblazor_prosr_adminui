using BlazorHub.Client.Business;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using prosr.Parser;
using prosr.Parser.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorHub.Client.Pages
{
    public partial class Prosr
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IBuilder Builder { get; set; }
        [Inject] public ILogger<Prosr> Logger { get; set; }
        [Inject] public IAstStore AstStore { get; set; }


        public Ast Ast { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var prosr = await HttpClient.GetStringAsync("api/helloworld/prosr");

            Logger.LogError(prosr);

            var ast = Builder.Build(prosr);

            AstStore.Store(ast);
            ast.GetType().GetMembers();
            Logger.LogError(JsonSerializer.Serialize(ast));

            Ast = ast;
        }

    }
}
