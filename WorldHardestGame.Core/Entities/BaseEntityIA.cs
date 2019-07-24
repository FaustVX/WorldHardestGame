using WorldHardestGame.Core.IA;

namespace WorldHardestGame.Core.Entities
{
    public abstract class BaseEntityIA : BaseEntity
    {
        public BaseEntityIA(Position position, BaseIA ia)
            : base(position)
        {
            IA = ia;
        }

        public BaseIA IA { get; }
    }
}
