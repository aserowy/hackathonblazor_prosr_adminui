using Microsoft.AspNetCore.Components;
using prosr.Parser;
using prosr.Parser.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorHub.Client.Pages
{
    public partial class Prosr
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IBuilder Builder { get; set; }

        public Ast Ast { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var prosr = await HttpClient.GetStringAsync("api/helloworld/prosr");
            var ast = Builder.Build(prosr);

            Ast = ast;
        }

    }
}
