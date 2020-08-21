using System.Collections.Generic;

namespace prosr.Parser.Models
{
    public sealed class Hub : INode
    {
        public Hub(string ident)
        {
            Ident = ident;

            Nodes = new List<INode>();
        }

        public Token Token => Token.Hub;

        public string Ident { get; set; }
        public IEnumerable<INode> Nodes { get; set; }
    }
}
