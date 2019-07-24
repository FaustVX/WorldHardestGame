using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldHardestGame.Core;
using System.IO;

namespace WorldHardestGame.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using var fileStream = File.OpenText(@"D:\OneDrive\Visual Studio\Projects\WorldHardestGame\Map1.whg.map");
            var map = Map.Parse(fileStream);
            Assert.IsNotNull(map);
            Assert.AreEqual("Map 1", map.Name);
            Assert.AreEqual(new Size(20, 8), map.Size);
        }
    }
}
