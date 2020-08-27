using prosr.Parser.Models;

namespace BlazorHub.Client.Business
{
    public interface IMessageResolverFactory
    {
        IMessageResolver Create(Ast ast);
    }

    internal sealed class MessageResolverFactory : IMessageResolverFactory
    {
        private readonly IMessageTypeFactory _messageTypeFactory;

        public MessageResolverFactory(IMessageTypeFactory messageTypeFactory)
        {
            _messageTypeFactory = messageTypeFactory;
        }

        public IMessageResolver Create(Ast ast)
        {
            return new MessageResolver(_messageTypeFactory, ast);
        }
    }
}
