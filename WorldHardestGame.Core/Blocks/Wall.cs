using System;
using System.Collections.Generic;
using System.Text;

namespace WorldHardestGame.Core.Blocks
{
    public class Wall : BaseBlock
    {
        public static Wall Instance { get; } = new Wall();
        public Wall()
        {

        }
    }
}
