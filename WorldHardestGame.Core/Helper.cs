using System;
using System.Diagnostics.CodeAnalysis;
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

        public static void ModifyReadOnlyProperty<T>(Expression<Func<T>> expression, in T value)
        {
            dynamic d = expression.Compile().Target!;
            var t = (object)d.Constants[0];
            if (expression.Body is MemberExpression prop)
                if (prop.Member is PropertyInfo propInfo)
                    if ((propInfo.CanRead && !propInfo.CanWrite))
                        if (propInfo.GetMethod!.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) is { })
                        {
                            var backingField = t.GetType().GetField($@"<{propInfo.Name}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
                            backingField!.SetValue(t, value);
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
