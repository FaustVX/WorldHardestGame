using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WorldHardestGame.Core
{
    public readonly struct Size : IXmlReaderSerializable
    {
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public readonly int Width;
        public readonly int Height;

        unsafe void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.AttributeCount >= 2 && GetIntAttribute("width", out var w) && GetIntAttribute("height", out var h))
            {
                fixed (int* width = &Width)
                    *width = w;
                fixed (int* height = &Height)
                    *height = h;
            }

            bool GetIntAttribute(string name, out int value)
                => reader.GetIntAttribute(name, out value);
        }
    }
}