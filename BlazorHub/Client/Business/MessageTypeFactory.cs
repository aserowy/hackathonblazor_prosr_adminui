using prosr.Parser.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BlazorHub.Client.Business
{
    internal interface IMessageTypeFactory
    {
        Type Create(IDictionary<string, Type> types, Message message);
    }

    internal sealed class MessageTypeFactory : IMessageTypeFactory
    {
        public Type Create(IDictionary<string, Type> types, Message message)
        {
            var typeBuilder = CreateTypeBuilder(message.Ident);
            foreach (var field in message.Nodes)
            {
                CreateProperty(typeBuilder, field.Ident, ResolveType(types, field.Type, field.IsRepeated));
            }

            return typeBuilder.CreateType();
        }

        private TypeBuilder CreateTypeBuilder(string ident)
        {
            var assemblyName = new AssemblyName(ident);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Dynamic.Models");
            var typeBuilder = moduleBuilder.DefineType(
                assemblyName.FullName,
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout,
                null);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            return typeBuilder;
        }

        private Type ResolveType(IDictionary<string, Type> types, string type, bool isRepeated)
        {
            var result = types[type];
            if (isRepeated)
            {
                result = typeof(List<>).MakeGenericType(result);
            }

            return result;
        }

        private void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            var fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            var getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            var getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            var setIl = setPropMthdBldr.GetILGenerator();
            var modifyProperty = setIl.DefineLabel();
            var exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
