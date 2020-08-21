using Microsoft.Extensions.Logging;
using prosr.Parser.Models;
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
        private readonly ILogger<MessageResolver> _logger;

        public MessageResolver(
            IAstStore astStore,
            ILogger<MessageResolver> logger)
        {
            _astStore = astStore;
            _logger = logger;
        }
        public Message GetMessageByName(string ident)
        {
            return _astStore
                .Ast
                .Nodes
                .OfType<Message>()
                .Single(x => x.Ident == ident);
        }
    }
}
