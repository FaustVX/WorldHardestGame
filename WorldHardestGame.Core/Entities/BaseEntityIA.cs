using WorldHardestGame.Core.IA;

namespace WorldHardestGame.Core.Entities
{
    public abstract class BaseEntityIA : BaseEntity
    {
        public BaseEntityIA(Position position, BaseIA? ia, Rectangle boundingBox, Map map)
            : base(position, boundingBox, map)
        {
            IA = ia;
        }

        public BaseIA? IA { get; }
    }
}
