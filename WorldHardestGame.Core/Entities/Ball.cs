using System;
using System.Collections.Generic;
using System.Text;

namespace WorldHardestGame.Core.Entities
{
    public class Ball : BaseEntityIA
    {
        public Ball(Position position, IA.BaseIA ia)
            : base(position, ia)
        {

        }

        protected override void ExecuteImpl(TimeSpan time)
            => IA.Execute(time);
    }
}
