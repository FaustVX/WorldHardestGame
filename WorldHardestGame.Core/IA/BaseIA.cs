using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public abstract class BaseIA
    {
        public BaseIA(BaseEntityIA entity, float duration)
        {
            Entity = entity;
            TotalDuration = duration;
        }

        public BaseEntityIA Entity { get; }
        public float TotalDuration { get; }
        
        private float currentDuration;
        protected float CurrentDuration
        {
            get => currentDuration;
            private set => currentDuration = value % TotalDuration;
        }

        public void Update(TimeSpan deltaTime)
            => UpdateImpl((CurrentDuration += (float)deltaTime.TotalSeconds) / TotalDuration);

        protected abstract void UpdateImpl(float timePos);
    }
}