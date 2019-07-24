using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WorldHardestGame.Core
{
    public class Map : IXmlReaderSerializable
    {
        public Map(string name, Size size, IEnumerable<Blocks.BaseBlock> blocks, IEnumerable<Entities.BaseEntity> entities)
        {
            Name = name;
            Size = size;
        }

        private Map()
        {
            Name = "";
            Size = default;
        }

        public string Name { get; }
        public Size Size { get; }

        public static Map Parse(StreamReader fileStream)
        {
            using var xmlReader = XmlReader.Create(fileStream);
            return (Map)new XmlSerializer(typeof(Map)).Deserialize(xmlReader);
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType is XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Header":
                            reader.ReadSubTree(ReadHeader);
                            break;
                        case "Blocks":
                            reader.ReadSubTree(ReadBlocks);
                            break;
                        case "Entities":
                            reader.ReadSubTree(ReadEntities);
                            break;
                    }
                }
            }

            void ReadHeader(XmlReader reader)
            {
                var setName = false;
                var setSize = false;
                while (!(setName && setSize) && reader.Read())
                {
                    if (reader.NodeType is XmlNodeType.Element)
                        if (reader.Name is nameof(Name))
                        {
                            var name = reader.ReadElementContentAsString();
                            Helper.ModifyReadOnlyProperty(() => Name, name);

                            if (setSize)
                                break;
                            setName = true;
                        }
                        else if (reader.Name is nameof(Size))
                        {
                            var size = reader.Deserialize<Size>();
                            Helper.ModifyReadOnlyProperty(() => Size, size);
                            Helper.ModifyReadOnlyProperty(() => Blocks, new BaseBlock[Size.Width, Size.Height]);

                            foreach (var x in Enumerable.Range(0, Size.Width))
                                foreach (var y in Enumerable.Range(0, Size.Height))
                                    Blocks[x, y] = Wall.Instance;

                            if (setName)
                                break;
                            setSize = true;
                        }
                }
            }

            void ReadBlocks(XmlReader reader)
            {
            }

            void ReadEntities(XmlReader reader)
            {
            }
        }
    }
}
