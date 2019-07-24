using System;
using System.Collections.Generic;
using System.Text;

namespace WorldHardestGame.Core.Entities
{
    public class Ball : BaseEntityIA
    {
        public Ball(Position position, IA.BaseIA ia, Rectangle boundingBox, Map map)
            : base(position, ia, boundingBox, map)
        {

        }

        protected override void ExecuteImpl(TimeSpan time)
            => IA.Execute(time);
        protected override bool HasContactWith(TimeSpan time, Player player)
        {
            return true;
        }
    }
}
