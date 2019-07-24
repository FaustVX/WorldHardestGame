using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WorldHardestGame.Core
{
    public readonly struct Position : IXmlReaderSerializable
    {
        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public readonly float X;
        public readonly float Y;

        unsafe void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.AttributeCount >= 2 && GetFloatAttribute("x", out var w) && GetFloatAttribute("y", out var h))
            {
                fixed (float* x = &X)
                    *x = w;
                fixed (float* y = &Y)
                    *y = h;
            }

            bool GetFloatAttribute(string name, out float value)
                => reader.GetFloatAttribute(name, out value);
        }

        public static Position operator +(in Position left, in Position right)
            => new Position(left.X + right.X, left.Y + right.Y);
    }
}