using Antlr4.Runtime;
using prosr.Parser.Compiler;
using prosr.Parser.Models;

namespace prosr.Parser
{
    public interface IBuilder
    {
        Ast Build(string prosr);
    }

    public sealed class Builder : IBuilder
    {
        public Ast Build(string prosr)
        {
            var input = new AntlrInputStream(prosr);
            var lexer = new Prosr1Lexer(input);
            var token = new CommonTokenStream(lexer);
            var parser = new Prosr1Parser(token);
            parser.BuildParseTree = true;

            var listener = new Prosr1Listener();
            parser.AddParseListener(listener);
            parser.content();

            return listener.GetAst();
        }
    }
}
