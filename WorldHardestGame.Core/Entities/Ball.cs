using System;

namespace WorldHardestGame.Core.Entities
{
    public class Ball : BaseEntityIA
    {
        public Ball(Position position, IA.BaseIA ia, Rectangle boundingBox, Map map)
            : base(position, ia, boundingBox, map)
        {

        }

        public override bool IsEnnemy
            => true;

        protected override void UpdateImpl(TimeSpan deltaTime)
            => IA?.Update(deltaTime);

        protected override bool HasContactWith(Player player)
        {
            return true;
        }
    }
}
