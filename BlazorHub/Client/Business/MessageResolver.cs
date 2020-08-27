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
        private readonly IMessageTypeFactory _typeFactory;
        private readonly Ast _ast;

        public MessageResolver(IMessageTypeFactory typeFactory, Ast ast)
        {
            _typeFactory = typeFactory;
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

        public Type GetType(string ident)
        {
            return _typeFactory.Create(GetByName(ident));
        }

        public object GetObject(string ident, string json)
        {
            return JsonSerializer.Deserialize(json, GetType(ident));
        }
    }
}
