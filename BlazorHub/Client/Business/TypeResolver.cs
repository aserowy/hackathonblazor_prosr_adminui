using prosr.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorHub.Client.Business
{
    public interface ITypeResolver
    {
        void Initialize(Ast ast);
        Type Get(string type, bool isRepeated);
    }

    internal sealed class TypeResolver : ITypeResolver
    {
        private static readonly IDictionary<string, Type> _types = new Dictionary<string, Type>
        {
            ["bool"] = typeof(bool),
            ["int32"] = typeof(int),
            ["string"] = typeof(string)
        };

        private readonly IMessageTypeFactory _factory;

        public TypeResolver(IMessageTypeFactory factory)
        {
            _factory = factory;
        }

        public void Initialize(Ast ast)
        {
            var ordered = ResolveMessagesByDependency(ast);
            foreach (var message in ordered)
            {
                if (_types.ContainsKey(message.Ident))
                {
                    continue;
                }

                _types[message.Ident] = _factory.Create(_types, message);
            }
        }

        public Type Get(string type, bool isRepeated)
        {
            return _types[type];
        }

        private IEnumerable<Message> ResolveMessagesByDependency(Ast ast)
        {
            var messages = ResolveMessages(ast);
            var graph = BuildGraph(messages);
            var ordered = WalkGraph(graph);

            return ordered.Select(nd => nd.Message);
        }

        private IEnumerable<Message> ResolveMessages(Ast ast)
        {
            var msgInPkgs = ast
                .Nodes
                .OfType<Package>()
                .SelectMany(pkg => pkg.Nodes)
                .OfType<Message>();

            var messages = ast
                .Nodes
                .OfType<Message>()
                .Union(msgInPkgs);

            return messages;
        }

        private IEnumerable<Node> BuildGraph(IEnumerable<Message> messages)
        {
            var result = new List<Node>();
            foreach (var message in messages)
            {
                var isCurrentNodeRoot = true;
                var currentNode = new Node(message);

                foreach (var node in result)
                {
                    if (currentNode.Fields.Contains(node.Message.Ident))
                    {
                        currentNode.Nodes.Add(node);
                    }

                    if (node.Fields.Contains(currentNode.Message.Ident))
                    {
                        isCurrentNodeRoot = false;

                        node.Nodes.Add(currentNode);
                    }
                }

                if (isCurrentNodeRoot)
                {
                    result.Add(currentNode);
                }

                foreach (var node in currentNode.Nodes)
                {
                    result.Remove(node);
                }
            }

            return result;
        }

        private IEnumerable<Node> WalkGraph(IEnumerable<Node> graph)
        {
            var result = new List<Node>() as IList<Node>;
            foreach (var node in graph)
            {
                WalkNode(node, ref result);
            }

            return result;
        }

        private void WalkNode(Node node, ref IList<Node> resolved)
        {
            if (node.IsWalked)
            {
                return;
            }

            node.IsWalked = true;

            foreach (var dependency in node.Nodes)
            {
                WalkNode(dependency, ref resolved);
            }

            resolved.Add(node);
        }

        private class Node
        {
            public Node(Message message, IList<Node> nodes = null)
            {
                Message = message;
                Nodes = nodes ?? new List<Node>();

                Fields = new HashSet<string>(message
                    .Nodes
                    .Select(fld => fld.Type)
                    .Distinct());
            }

            public Message Message { get; set; }
            public bool IsWalked { get; set; }
            public IList<Node> Nodes { get; set; }

            public HashSet<string> Fields { get; }
        }
    }
}
