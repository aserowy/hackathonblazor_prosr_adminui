using System.Collections.Generic;

namespace prosr.Parser.Models
{
    public sealed class Message : INode
    {
        public Message(string ident)
        {
            Ident = ident;

            Nodes = new List<Field>();
        }

        public Token Token => Token.Message;

        public string Ident { get; set; }
        public IList<Field> Nodes { get; set; }
    }
}
