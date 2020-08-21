using System.Collections.Generic;

namespace prosr.Parser.Models
{
    public sealed class Package : INode
    {
        public Package(string ident)
        {
            Ident = ident;

            Nodes = new List<INode>();
        }

        public Token Token => Token.Package;

        public string Ident { get; set; }
        public IEnumerable<INode> Nodes { get; set; }
    }
}
