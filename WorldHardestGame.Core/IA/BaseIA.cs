using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public abstract class BaseIA
    {
        public BaseIA(BaseEntityIA entity)
        {
            Entity = entity;
        }

        public BaseEntityIA Entity { get; }

        public void Update(TimeSpan deltaTime)
            => UpdateImpl(deltaTime);

        protected abstract void UpdateImpl(TimeSpan deltaTime);
    }
}