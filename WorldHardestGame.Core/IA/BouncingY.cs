using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public class BouncingY : BaseMovingIA
    {
        public BouncingY(float start, float end, float duration, BaseEntityIA entity)
            : base(entity, duration)
        {
            Start = start;
            End = end;
            Length = End - Start;
        }

        public float Start { get; }
        public float End { get; }
        public float Length { get; }

        private void ModifyYPosition(float pos)
            => Entity.Position = new Position(pos * Length / 2 + Start, Entity.Position.Y);

        protected override void UpdateImpl(float timePosition)
            => ModifyYPosition(-MathF.Cos(timePosition * MathF.PI * 2) + 1);
    }
}
