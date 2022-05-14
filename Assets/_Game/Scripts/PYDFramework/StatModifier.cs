using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYDFramework
{
    public enum StatModifierType
    {
        Add,
        Mul,
    }

    public abstract class StatModifier<T>
    {
        public T value;
        public StatModifierType type;
        public object source;

        public StatModifier(T value, StatModifierType type, object source)
        {
            this.value = value;
            this.type = type;
            this.source = source;
        }
    }

    public class StatModifierDouble : StatModifier<double>
    {
        public StatModifierDouble(double value, StatModifierType type, object source) : base(value, type, source)
        {
        }
    }

    public class StatModifierFloat : StatModifier<float>
    {
        public StatModifierFloat(float value, StatModifierType type, object source) : base(value, type, source)
        {
        }
    }

    public class StatModifierInt : StatModifier<int>
    {
        public StatModifierInt(int value, StatModifierType type, object source) : base(value, type, source)
        {
        }
    }
}
