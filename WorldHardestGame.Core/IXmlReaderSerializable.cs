using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WorldHardestGame.Core
{
    public interface IXmlReaderSerializable : IXmlSerializable
    {
        XmlSchema? IXmlSerializable.GetSchema()
            => null;

        void IXmlSerializable.WriteXml(XmlWriter writer)
            => throw new NotImplementedException();
    }
}
