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

        public void Execute(TimeSpan time)
            => ExecuteImpl(time);

        protected abstract void ExecuteImpl(TimeSpan time);
    }
}