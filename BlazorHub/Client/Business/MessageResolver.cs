using prosr.Parser.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlazorHub.Client.Business
{
    public interface IMessageResolver
    {
        Message GetMessageByName(string ident);
    }

    internal class MessageResolver : IMessageResolver
    {
        private readonly IAstStore _astStore;

        public MessageResolver(IAstStore astStore)
        {
            _astStore = astStore;
        }

        public Message GetMessageByName(string ident)
        {
            return _astStore
                .Ast
                .Nodes
                .SelectMany(x => x is Package package
                                 ? package.Nodes
                                 : new List<INode>() { x })
                .OfType<Message>()
                .SingleOrDefault(x => x.Ident == ident);
        }
    }
}
