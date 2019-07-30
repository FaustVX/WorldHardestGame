using System.Collections.Generic;
using System.Linq;
using WorldHardestGame.Core.Entities;

namespace WorldHardestGame.Core.IA
{
    public class Path : BaseMovingIA
    {
        public Path(BaseEntityIA entity, float duration, float distance, IEnumerable<Position> positions)
            : base(entity, duration)
        {
            Positions = new List<Position>(positions.Prepend(entity.Position));
            Sections = new List<(Position start, Position end, float timeStart, float timeEnd)>(Positions.Count);

            var previous = 0f;
            foreach (var (first, second) in EnumerateBy2(Positions.Append(Positions[0])))
                Sections.Add((first, second, previous, (previous += first.DistanceWith(second) / distance * duration)));


            static IEnumerable<(T first, T second)> EnumerateBy2<T>(IEnumerable<T> enumerable)
            {
                var enumerator = enumerable.GetEnumerator();

                enumerator.MoveNext();
                var last = enumerator.Current;
                while (enumerator.MoveNext())
                    yield return (last, last = enumerator.Current);
            }
        }

        public List<Position> Positions { get; }
        public List<(Position start, Position end, float timeStart, float timeEnd)> Sections { get; }

        protected override void ContactWithImpl(Player player)
        { }

        protected override void UpdateImpl(float timePos)
        {
            timePos %= 1f;
            var time = timePos * TotalDuration;
            var section = Sections[Sections.FindIndex(sec => time <= sec.timeEnd)];
            time -= section.timeStart;
            Entity.Position = Lerp(section.start, section.end, time / (section.timeEnd - section.timeStart));
        }

        private static Position Lerp(Position start, Position end, float percentage)
            => new Position((end.X - start.X) * percentage + start.X, (end.Y - start.Y) * percentage + start.Y);
    }
}
