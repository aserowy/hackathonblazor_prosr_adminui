using prosr.Parser.Models;

namespace BlazorHub.Client.Business
{
    public interface IMessageResolverFactory
    {
        IMessageResolver Create(Ast ast);
    }

    internal sealed class MessageResolverFactory : IMessageResolverFactory
    {
        private readonly ITypeResolver _typeResolver;

        public MessageResolverFactory(ITypeResolver typeResolver)
        {
            _typeResolver = typeResolver;
        }

        public IMessageResolver Create(Ast ast)
        {
            return new MessageResolver(_typeResolver, ast);
        }
    }
}
