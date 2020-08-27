using prosr.Parser.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlazorHub.Client.Business
{
    public interface IMessageResolver
    {
        Message GetByName(string ident);
    }

    internal class MessageResolver : IMessageResolver
    {
        private readonly Ast _ast;

        public MessageResolver(Ast ast)
        {
            _ast = ast;
        }

        public Message GetByName(string ident)
        {
            return _ast
                .Nodes
                .SelectMany(x => x is Package package ? package.Nodes : new List<INode>() { x })
                .OfType<Message>()
                .SingleOrDefault(x => x.Ident == ident);
        }
    }
}
