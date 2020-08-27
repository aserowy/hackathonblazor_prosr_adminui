using System;
using System.Collections.Generic;

namespace BlazorHub.Client.Business
{
    public interface IFieldTypeResolver
    {
        Type Get(string type, bool isRepeated);
    }

    internal sealed class FieldTypeResolver : IFieldTypeResolver
    {
        private static readonly IDictionary<string, Type> _defaultTypes = new Dictionary<string, Type>
        {
            ["string"] = typeof(string),
            ["int32"] = typeof(int)
        };

        public Type Get(string type, bool isRepeated)
        {
            return _defaultTypes[type];
        }
    }
}
