using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public class BouncingX : BaseIA
    {
        public BouncingX(float start, float end, float duration, BaseEntityIA entity)
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
            => Entity.Position = new Position(Entity.Position.X, pos * Length / 2 + Start);

        protected override void UpdateImpl(float timePosition)
            => ModifyYPosition(-MathF.Cos(timePosition * MathF.PI * 2) + 1);
    }
}
