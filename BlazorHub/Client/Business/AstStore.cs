using prosr.Parser.Models;

namespace BlazorHub.Client.Business
{
    public interface IAstStore
    {
        void Store(Ast ast);

        Ast Ast { get; }
    }

    internal class AstStore : IAstStore
    {
        public Ast Ast { get; private set; }

        public void Store(Ast ast)
        {
            Ast = ast;
        }
    }
}
