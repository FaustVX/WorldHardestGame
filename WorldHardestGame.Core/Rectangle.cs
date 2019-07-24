using System.Xml;

namespace WorldHardestGame.Core
{
    public readonly struct Rectangle : IXmlReaderSerializable
    {
        public readonly Position TopLeft;
        public readonly Position BottomRight;

        public Rectangle(Position topLeft, Position bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public unsafe void ReadXml(XmlReader reader)
        {
            var tl = reader.Deserialize<Position>();
            var br = reader.Deserialize<Position>();

            fixed (Position* topLeft = &TopLeft)
                *topLeft = tl;

            fixed (Position* bottomRight = &BottomRight)
                *bottomRight = br;
        }
    }
}