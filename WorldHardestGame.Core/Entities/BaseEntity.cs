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

        public virtual Position Position { get; set; }
        public Rectangle BoundingBox { get; }
        public Map Map { get; }

        public void Update(TimeSpan deltaTime)
            => UpdateImpl(deltaTime);

        protected abstract void UpdateImpl(TimeSpan deltaTime);

        protected abstract bool HasContactWith(Player player);

        protected static bool HasContactBetween(Player player, BaseEntity entity)
            => entity.HasContactWith(player);
    }
}
