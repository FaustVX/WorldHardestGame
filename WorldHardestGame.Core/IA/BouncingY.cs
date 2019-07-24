using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public class BouncingY : BaseIA
    {
        public BouncingY(float start, float end, float duration, BaseEntityIA entity)
            : base(entity)
        {
            Start = start;
            End = end;
            Length = End - Start;
            Duration = duration;
        }

        public float Start { get; }
        public float End { get; }
        public float Length { get; }
        public float Duration { get; }

        protected override void ExecuteImpl(TimeSpan time)
            => Entity.Position = new Position(-MathF.Cos((float)(time.TotalSeconds % Duration) * MathF.PI) * (Length / 2) + Length / 2 + Start
                , Entity.Position.Y);
    }
}
