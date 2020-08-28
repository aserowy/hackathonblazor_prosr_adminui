using prosr.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BlazorHub.Client.Business
{
    public interface IMessageResolver
    {
        Message GetByName(string ident);
        Type GetType(string ident);
        object GetObject(string ident, string json);
    }

    internal class MessageResolver : IMessageResolver
    {
        private readonly ITypeResolver _typeResolver;
        private readonly Ast _ast;

        public MessageResolver(ITypeResolver typeResolver, Ast ast)
        {
            _typeResolver = typeResolver;
            _ast = ast;

            _typeResolver.Initialize(_ast);
        }

        public Message GetByName(string ident)
        {
            return _ast
                .Nodes
                .SelectMany(nd => nd is Package package ? package.Nodes : new List<INode>() { nd })
                .OfType<Message>()
                .SingleOrDefault(msg => msg.Ident == ident);
        }

        public Type GetType(string ident)
        {
            return _typeResolver.Get(ident, false);
        }

        public object GetObject(string ident, string json)
        {
            return JsonSerializer.Deserialize(json, GetType(ident));
        }
    }
}
