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
            RespawnPosition = Position;
        }

        public override Position Position
        {
            get => base.Position;
            set
            {
                var (top, left, bottom, right) = Get4Corners(GetCorners(value));

                if (Map[left, top] is BlockType.Wall || Map[left, bottom] is BlockType.Wall || Map[right, top] is BlockType.Wall || Map[right, bottom] is BlockType.Wall)
                    return;
                base.Position = value;

                (Position tl, Position br) GetCorners(Position position)
                    => (position + BoundingBox.TopLeft, position + BoundingBox.BottomRight);

                static (int top, int left, int bottom, int right) Get4Corners(in (Position tl, Position br) corners)
                    => ((int)corners.tl.Y, (int)corners.tl.X, (int)corners.br.Y, (int)corners.br.X);
            }
        }

        public Position RespawnPosition { get; set; }

        public override bool IsEnnemy
            => false;

        public BaseEntity? HasBennKilledBy { get; private set; }

        protected override void UpdateImpl(TimeSpan deltaTime)
        {
            var me = Get4Corners(GetCorners(this));

            foreach (var entity in Map.NonKilledEntities)
            {
                if (entity is Player)
                    continue;

                var other = Get4Corners(GetCorners(entity));
                if (!(other.left > me.right || other.right < me.left || other.top > me.bottom || other.bottom < me.top))
                    if (HasContactBetween(this, entity) && !entity.IsKilled && entity.IsEnnemy)
                        Position = RespawnPosition;
            }

            switch (Map[Position])
            {
                case BlockType.Finish when Map.FinishedUnlocked:
                    Map.Finished = true;
                    break;
                case BlockType.Checkpoint:
                    RespawnPosition = new Position(.5f + (int)Position.X, .5f + (int)Position.Y);
                    break;
            }

            static (Position tl, Position br) GetCorners(BaseEntity entity)
                => (entity.Position + entity.BoundingBox.TopLeft, entity.Position + entity.BoundingBox.BottomRight);

            static (float top, float left, float bottom, float right) Get4Corners(in (Position tl, Position br) corners)
                => (corners.tl.Y, corners.tl.X, corners.br.Y, corners.br.X);
        }

        protected override bool HasContactWith(Player player)
            => throw new NotImplementedException();
    }
}
