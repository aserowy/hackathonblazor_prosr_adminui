using prosr.Parser.Models;
using System.Text.Json;

namespace BlazorHub.Client.Business
{
    public interface IObjectResolver
    {
        object Get(Message message, string json);
    }

    internal sealed class ObjectResolver : IObjectResolver
    {
        private readonly IMessageTypeFactory _typeFactory;

        public ObjectResolver(IMessageTypeFactory typeFactory)
        {
            _typeFactory = typeFactory;
        }

        public object Get(Message message, string json)
        {
            var type = _typeFactory.Create(message);
            var result = JsonSerializer.Deserialize(json, type);

            return result;
        }
    }
}
