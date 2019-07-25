using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public abstract class BaseMovingIA : BaseIA
    {
        public BaseMovingIA(BaseEntityIA entity, float duration)
            : base(entity)
        {
            TotalDuration = duration;
        }

        public float TotalDuration { get; }
        
        private float currentDuration;
        protected float CurrentDuration
        {
            get => currentDuration;
            private set => currentDuration = value % TotalDuration;
        }

        protected override void UpdateImpl(TimeSpan deltaTime)
            => UpdateImpl((CurrentDuration += (float)deltaTime.TotalSeconds) / TotalDuration);

        protected abstract void UpdateImpl(float timePos);
    }
}