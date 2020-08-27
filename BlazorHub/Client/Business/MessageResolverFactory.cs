using prosr.Parser.Models;

namespace BlazorHub.Client.Business
{
    public interface IMessageResolverFactory
    {
        IMessageResolver Create(Ast ast);
    }

    internal sealed class MessageResolverFactory : IMessageResolverFactory
    {
        public IMessageResolver Create(Ast ast)
        {
            return new MessageResolver(ast);
        }
    }
}
