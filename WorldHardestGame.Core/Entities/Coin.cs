using System;

namespace WorldHardestGame.Core.Entities
{
    public class Coin : BaseEntityIA
    {
        public Coin(Position position, IA.BaseIA ia, Rectangle boundingBox, Map map)
            : base(position, ia, boundingBox, map)
        {

        }

        protected override void UpdateImpl(TimeSpan deltaTime)
            => IA.Update(deltaTime);

        protected override bool HasContactWith(Player player)
        {
            IA.ContactWith(player);
            return true;
        }
    }
}
