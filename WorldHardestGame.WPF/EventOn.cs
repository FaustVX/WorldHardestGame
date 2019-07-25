using System;
using System.Collections.Generic;

namespace WorldHardestGame.WPF
{
    public class EventOn<TProperty>
    {
        public Func<TProperty> Property { get; }
        public Func<TProperty, TProperty, bool> Equal { get; }
        public TProperty Value { get; private set; }

        public EventOn(Func<TProperty> property, Func<TProperty, TProperty, bool> equal = null)
        {
            Property = property;
            Equal = equal ?? EqualityComparer<TProperty>.Default.Equals;
            Value = Property();
        }

        public bool HasChanged()
            => HasChanged(out _, out _);

        public bool HasChanged(out TProperty newProperty)
            => HasChanged(out _, out newProperty);

        public bool HasChanged(out TProperty oldProp, out TProperty newProperty)
            => !Equal(oldProp = Value, newProperty = Value = Property());
    }
}
