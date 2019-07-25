using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public class Collecting : BaseIA
    {
        private static Map _map;
        private static int _count;

        public Collecting(BaseEntityIA entity)
            : base(entity)
        {
            if (!ReferenceEquals(_map, Entity.Map))
            {
                _count = 0;
                _map = entity.Map;
                _map.FinishedUnlocked = false;
            }
            _count++;
        }

        protected override void ContactWithImpl(Player player)
        {
            if(--_count <= 0)
                _map.FinishedUnlocked = true;
            Entity.IsKilled = true;
        }

        protected override void UpdateImpl(TimeSpan deltaTime)
        { }
    }
}
