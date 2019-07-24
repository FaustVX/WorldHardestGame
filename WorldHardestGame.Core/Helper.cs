using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace WorldHardestGame.Core
{
    public static class Helper
    {
        public static void ReadSubTree(this XmlReader reader, Action<XmlReader> action)
        {
            using var subTree = reader.ReadSubtree();
            action(subTree);
        }

        public static T ReadSubTree<T>(this XmlReader reader, Func<XmlReader, T> func)
        {
            using var subTree = reader.ReadSubtree();
            return func(subTree);
        }

        public static T Deserialize<T>(this XmlReader reader)
            => (T)reader.ReadSubTree(new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize);

        public static bool ModifyReadOnlyProperty<TThis, TProperty>(this TThis @this, Expression<Func<TThis, TProperty>> expression, in TProperty value)
            where TThis : notnull
            => ModifyReadOnlyProperty<TProperty>(@this, expression.Body, value);

        public static bool ModifyReadOnlyProperty<T>(Expression<Func<T>> expression, in T value)
        {
            dynamic d = expression.Compile().Target!;
            var t = (object)d.Constants[0];
            return ModifyReadOnlyProperty(t, expression.Body, value);
        }

        private static bool ModifyReadOnlyProperty<T>(object @this, Expression body, in T value)
        {
            if (body is MemberExpression prop)
                if (prop.Member is PropertyInfo propInfo)
                    if ((propInfo.CanRead && !propInfo.CanWrite))
                        if (propInfo.GetMethod!.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) is { })
                        {
                            var backingField = @this.GetType().GetAllFields().FirstOrDefault(field => field.Name == $@"<{propInfo.Name}>k__BackingField");
                            if (backingField is null)
                                return false;
                            backingField.SetValue(@this, value);
                            return true;
                        }
            return false;
        }

        public static IEnumerable<FieldInfo> GetAllFields(this Type t)
        {
            return GetAllFieldsImpl(t);

            static IEnumerable<FieldInfo> GetAllFieldsImpl(Type? t)
            {
                if (t == null)
                    return Enumerable.Empty<FieldInfo>();

                var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
                return t.GetFields(flags).Concat(GetAllFieldsImpl(t.BaseType));
            }
        }

        public static bool GetBoolAttribute(this XmlReader reader, string name, out bool value)
            => bool.TryParse(reader.GetAttribute(name), out value);

        public static bool GetIntAttribute(this XmlReader reader, string name, out int value)
            => int.TryParse(reader.GetAttribute(name), out value);

        public static bool GetStringAttribute(this XmlReader reader, string name, /*[NotNullWhen(true)]*/ out string? value)
            => !string.IsNullOrWhiteSpace(value = reader.GetAttribute(name));

        public static bool GetFloatAttribute(this XmlReader reader, string name, out float value)
            => float.TryParse(reader.GetAttribute(name), out value);
    }
}
