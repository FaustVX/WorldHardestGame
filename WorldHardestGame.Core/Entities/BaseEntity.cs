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
    }
}
