using System;
using System.Collections.Generic;
using System.Text;

namespace WorldHardestGame.Core.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity(Position position)
        {
            Position = position;
        }

        public Position Position { get; set; }

        public void Execute(TimeSpan time)
            => ExecuteImpl(time);

        protected abstract void ExecuteImpl(TimeSpan time);
    }
}
