using System;

namespace WorldHardestGame.Core.Entities
{
    public class Coin : BaseEntityIA
    {
        private static Map _map;
        private static int _count;

        public Coin(Position position, IA.BaseIA? ia, Rectangle boundingBox, Map map)
            : base(position, ia, boundingBox, map)
        {
            if (!ReferenceEquals(_map, Map))
            {
                _count = 0;
                _map = Map;
                _map.FinishedUnlocked = false;
            }
            _count++;
        }

        public override bool IsEnnemy
            => false;

        protected override void UpdateImpl(TimeSpan deltaTime)
            => IA?.Update(deltaTime);

        protected override bool HasContactWith(Player player)
        {
            if (--_count <= 0)
                _map.FinishedUnlocked = true;
            IsKilled = true;

            return true;
        }
    }
}
