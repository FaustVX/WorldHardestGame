using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WorldHardestGame.Core.Blocks;

namespace WorldHardestGame.Core
{
    public class Map : IXmlReaderSerializable
    {
        private Map()
        {
            Name = "";
            Size = default;
            Blocks = new BaseBlock[0, 0];
        }

        public string Name { get; }
        public Size Size { get; }
        public BaseBlock[,] Blocks { get; }

        public BaseBlock this[Index x, Index y]
            => Blocks[x.GetOffset(Size.Width), y.GetOffset(Size.Height)];

        public static Map Parse(StreamReader fileStream)
        {
            using var xmlReader = XmlReader.Create(fileStream);
            return (Map)new XmlSerializer(typeof(Map)).Deserialize(xmlReader);
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var blocks = new Dictionary<string, (int x, int y, Size size)>();

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
                            return;
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
                (int x, int y, Size size) prevBlock = default;

                if (reader.NodeType is XmlNodeType.None)
                    reader.Read();

                while (reader.Read())
                {
                    if(reader.NodeType is XmlNodeType.Element)
                    {
                        if (!reader.GetIntAttribute("x", out var x))
                            throw new Exception("'x' attribute must be defined");
                        if (!reader.GetIntAttribute("y", out var y))
                            throw new Exception("'y' attribute must be defined");

                        if (reader.GetStringAttribute("id", out var id) && blocks.ContainsKey(id!))
                            throw new Exception($"id: '{id}' must be unique");

                        var w = reader.GetIntAttribute("width", out var value) ? (int?)value : null;
                        var h = reader.GetIntAttribute("height", out value) ? (int?)value : null;
                        if (w.HasValue ^ h.HasValue)
                            throw new Exception("Neither or Both 'width' and 'height' must be set");
                        else if (!w.HasValue && !h.HasValue && reader.GetIntAttribute("size", out var size))
                            w = h = size;

                        (var prev, string? relativeTo) = (false, null);
                        if (reader.GetStringAttribute(nameof(relativeTo), out relativeTo)
                            && !(prev = relativeTo == nameof(prev))
                            && !blocks.ContainsKey(relativeTo!))
                            throw new Exception($"id: '{relativeTo}' must be set before usage");
                        else if (prev)
                            relativeTo = null;

                        switch (reader.Name)
                        {
                            case nameof(Start):
                                SetBlocks(new Start(), x, y);
                                break;
                            case nameof(Finish):
                                SetBlocks(new Finish(), x, y);
                                break;
                            case nameof(Floor):
                                SetBlocks(new Floor(), x, y);
                                break;
                            case nameof(Wall):
                                SetBlocks(new Wall(), x, y);
                                break;
                        }

                        void SetBlocks(BaseBlock block, int x, int y)
                        {
                            if (prev || relativeTo is { })
                            {
                                var relativeToBlock = prev ? prevBlock : blocks[relativeTo];
                                (x, y) = (x + relativeToBlock.x, y + relativeToBlock.y);
                            }

                            if (!w.HasValue && !h.HasValue)
                            {
                                Blocks[x, y] = block;
                                prevBlock = (x, y, new Size(1, 1));
                            }
                            else
                            {
                                foreach (var i in Enumerable.Range(x, w.Value))
                                    foreach (var j in Enumerable.Range(y, h.Value))
                                        Blocks[i, j] = block;

                                prevBlock = (x, y, new Size(w.Value, h.Value));
                            }

                            if (id is { })
                                blocks.Add(id, prevBlock);
                        }
                    }
                }
            }

            void ReadEntities(XmlReader reader)
            {
            }
        }
    }
}
