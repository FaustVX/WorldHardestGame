using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldHardestGame.Core;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace WorldHardestGame.Test
{
    [TestClass]
    public class UnitTest1
    {
        private Map Map0;

        private Map InitializeMap()
        {
            using var fileStream = File.OpenText(@"D:\OneDrive\Visual Studio\Projects\WorldHardestGame\Map1.whg.map");
            return Map.Parse(fileStream);
        }

        [TestInitialize]
        public void Initialize()
        {
            Map0 = InitializeMap();
            DrawMap(Map0);
        }

        [TestMethod]
        public void ParseMap()
            => InitializeMap();

        [TestMethod]
        public void TestSampleBlock()
        {
            Assert.IsInstanceOfType(Map0[1, 1], typeof(Core.Blocks.Start));
            Assert.IsInstanceOfType(Map0[Map0.Size.Width / 2, Map0.Size.Height / 2], typeof(Core.Blocks.Floor));
            Assert.IsInstanceOfType(Map0[^2, ^2], typeof(Core.Blocks.Finish));
        }

        [TestMethod]
        public void MapIsNotNull()
        {
            Assert.IsNotNull(Map0);
        }

        [TestMethod]
        public void MapHeader()
        {
            Assert.AreEqual("Map 1", Map0.Name);
            Assert.AreEqual(new Size(20, 8), Map0.Size);
            Assert.AreEqual(20, Map0.Blocks.GetLength(0));
            Assert.AreEqual(8, Map0.Blocks.GetLength(1));
        }

        [TestMethod]
        public void WallAllAroundTheMap()
        {
            for (var x = 0; x < Map0.Size.Width; x++)
            {
                Assert.IsInstanceOfType(Map0[x, 0], typeof(Core.Blocks.Wall));
                Assert.IsInstanceOfType(Map0[x, ^1], typeof(Core.Blocks.Wall));
            }
            for (var y = 0; y < Map0.Size.Height; y++)
            {
                Assert.IsInstanceOfType(Map0[0, y], typeof(Core.Blocks.Wall));
                Assert.IsInstanceOfType(Map0[^1, y], typeof(Core.Blocks.Wall));
            }
        }

        [TestMethod]
        public void PlayerPosition()
        {
            Assert.AreEqual(float.Parse("2.5"), Map0.Entities.OfType<Core.Entities.Player>().Single().Position.X);
            Assert.AreEqual(float.Parse("4"), Map0.Entities.OfType<Core.Entities.Player>().Single().Position.Y);
        }

        private static void DrawMap(Map map)
        {
            Debug.WriteLine(map.Name);

            for (var y = 0; y < map.Size.Height; y++)
            {
                for (var x = 0; x < map.Size.Width; x++)
                {
                    var writen = true;

                    switch (map.Entities.FirstOrDefault(e => (int)e.Position.X == x && (int)e.Position.Y == y))
                    {
                        case Core.Entities.Player _:
                            Debug.Write('@');
                            break;
                        case Core.Entities.Ball _:
                            Debug.Write('O');
                            break;
                        default:
                            writen = false;
                            break;
                    }

                    if (writen)
                        continue;

                    switch (map[x, y])
                    {
                        case Core.Blocks.Wall _:
                            Debug.Write('#');
                            break;
                        case Core.Blocks.Floor _:
                            Debug.Write('.');
                            break;
                        case Core.Blocks.Start _:
                            Debug.Write('s');
                            break;
                        case Core.Blocks.Finish _:
                            Debug.Write('f');
                            break;
                    }
                }
                Debug.WriteLine("");
            }
        }
    }
}
