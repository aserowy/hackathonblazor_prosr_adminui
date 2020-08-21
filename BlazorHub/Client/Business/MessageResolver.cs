using BlazorHub.Client.Tmp;
using Microsoft.Extensions.Logging;
using prosr.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHub.Client.Business
{
    public interface IMessageResolver
    {
        Message GetMessageByName(string ident);
    }

    internal class MessageResolver : IMessageResolver
    {
        private readonly ITmpModelFactory _tmpModelFactory;
        private readonly ILogger<MessageResolver> _logger;

        public MessageResolver(
            ITmpModelFactory tmpModelFactory,
            ILogger<MessageResolver> logger)
        {
            _tmpModelFactory = tmpModelFactory;
            _logger = logger;
        }
        public  Message GetMessageByName(string ident)
        {
            _logger.LogError(ident);
            return _tmpModelFactory
                .Create()
                .Nodes
                .OfType<Message>()
                .Single(x => x.Ident == ident);
        }
    }
}
