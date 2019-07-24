using System;
using System.Collections.Generic;
using System.Text;

namespace WorldHardestGame.Core.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity(Position position, Rectangle boundingBox, Map map)
        {
            Map = map;
            BoundingBox = boundingBox;
            Position = position;
        }

        public Position Position { get; set; }
        public Rectangle BoundingBox { get; }
        public Map Map { get; }

        public void Execute(TimeSpan time)
            => ExecuteImpl(time);

        protected abstract void ExecuteImpl(TimeSpan time);

        protected abstract bool HasContactWith(TimeSpan time, Player player);

        protected static bool HasContactBetween(TimeSpan time, Player player, BaseEntity entity)
            => entity.HasContactWith(time, player);
    }
}
