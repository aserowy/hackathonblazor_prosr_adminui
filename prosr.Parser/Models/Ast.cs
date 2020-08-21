using System.Collections.Generic;

namespace prosr.Parser.Models
{
    public sealed class Ast
    {
        public Ast()
        {
            Nodes = new List<INode>();
        }

        public IEnumerable<INode> Nodes { get; set; }
    }
}
