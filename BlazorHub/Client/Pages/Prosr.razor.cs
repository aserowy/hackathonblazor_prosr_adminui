using BlazorHub.Client.Tmp;
using Microsoft.AspNetCore.Components;
using prosr.Parser.Models;
using System.Threading.Tasks;

namespace BlazorHub.Client.Pages
{
    public partial class Prosr
    {
        [Inject()]
        public ITmpModelFactory ModelFactory { get; set; }

        public Ast Ast { get; set; }

        protected override Task OnInitializedAsync()
        {
            this.Ast = ModelFactory.Create();
            return Task.CompletedTask;
        }

    }
}
