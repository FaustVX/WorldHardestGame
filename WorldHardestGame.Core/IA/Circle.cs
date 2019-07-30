using System;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public class Circle : BaseMovingIA
    {
        public Circle(BaseEntityIA entity, float duration, Position center)
            : base(entity, duration)
        {
            Center = center;
            StartPosition = entity.Position;
        }

        public Position Center { get; }
        public Position StartPosition { get; }

        protected override void UpdateImpl(float timePos)
        {
            Entity.Position = RotatePoint(timePos * 2 * MathF.PI - (MathF.PI / 2), StartPosition);
        }

        private Position RotatePoint(float angle, Position p)
        {
            var s = MathF.Sin(angle);
            var c = MathF.Cos(angle);

            // translate point back to origin:
            p -= new Position(Center.X, Center.Y);

            // rotate point
            var xnew = p.X * c - p.Y * s;
            var ynew = p.X * s + p.Y * c;

            // translate point back:
            return new Position(xnew + Center.X, ynew + Center.Y);
        }
    }
}
