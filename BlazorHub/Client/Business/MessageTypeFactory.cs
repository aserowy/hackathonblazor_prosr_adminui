using prosr.Parser.Models;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace BlazorHub.Client.Business
{
    internal interface IMessageTypeFactory
    {
        Type Create(Message message);
    }

    internal sealed class MessageTypeFactory : IMessageTypeFactory
    {
        public Type Create(Message message)
        {
            var typeBuilder = CreateTypeBuilder(message.Ident);

            return typeBuilder.CreateType();
        }

        private TypeBuilder CreateTypeBuilder(string ident)
        {
            var assemblyName = new AssemblyName($"{GetType().AssemblyQualifiedName}.{ident}");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Dynamic.Models");
            var typeBuilder = moduleBuilder.DefineType(
                assemblyName.FullName,
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout,
                null);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            return typeBuilder;
        }
    }
}
