using System;
using System.Collections.Generic;
using System.Text;

namespace WorldHardestGame.Core.Entities
{
    public class Player : BaseEntity
    {
        public Player(Position position, Rectangle boundingBox, Map map)
            : base(position, boundingBox, map)
        {

        }

        public BaseEntity? HasBennKilledBy { get; private set; }

        protected override void ExecuteImpl(TimeSpan time)
        {
            var me = Get4Corners(GetCorners(this));

            foreach (var entity in Map.Entities)
            {
                if (entity is Player)
                    continue;

                var other = Get4Corners(GetCorners(entity));
                if (other.left>me.right || other.right<me.left || other.top > me.bottom || other.bottom < me.top)
                    continue;
            }

            if (HasContactBetween(time, this, entity))
                HasBennKilledBy = entity;

            static (Position tl, Position br) GetCorners(BaseEntity entity)
                => (entity.Position + entity.BoundingBox.TopLeft, entity.Position + entity.BoundingBox.BottomRight);

            static (float top, float left, float bottom, float right) Get4Corners(in (Position tl, Position br) corners)
                => (corners.tl.Y, corners.tl.X, corners.br.Y, corners.br.X);
        }

        protected override bool HasContactWith(TimeSpan time, Player player)
            => throw new NotImplementedException();
    }
}
