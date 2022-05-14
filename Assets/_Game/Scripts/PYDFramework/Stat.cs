using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYDFramework
{
    public abstract class Stat<T>
    {
        protected T _baseValue;
        protected List<StatModifier<T>> modifiers = new List<StatModifier<T>>();

        public void AddModifier(StatModifier<T> modifier)
        {
            modifiers.Add(modifier);
        }

        public void RemoveModifier(StatModifier<T> modifier)
        {
            modifiers.Remove(modifier);
        }

        public void RemoveAllModifier()
        {
            modifiers.Clear();
        }

        public void RemoveAllModifierFromSource(object source)
        {
            modifiers.RemoveAll(m => m.source == source);
        }

        public T BaseValue
        {
            get => _baseValue;
            set => _baseValue = value;
        }

        /// <summary>
        /// Value after caculation. Mul first Add last
        /// </summary>
        public T Value => CaculatorValue();

        protected virtual T CaculatorValue() { return default(T); }

        protected int CompareModifier(StatModifier<T> x, StatModifier<T> y)
        {
            if ((int)x.type > (int)y.type)
                return 1;
            else if ((int)x.type < (int)y.type)
                return -1;
            else
                return 0;
        }
    }

    public class StatDouble: Stat<double>
    {
        protected override double CaculatorValue()
        {
            modifiers.Sort(CompareModifier);

            var final = BaseValue;
            foreach(var modifier in modifiers)
            {
                if (modifier.type == StatModifierType.Add)
                    final += modifier.value;
                else if (modifier.type == StatModifierType.Mul)
                    final *= modifier.value;
            }
            return final;
        }


        public StatDouble Clone()
        {
            var result = (StatDouble)this.MemberwiseClone();
            result.modifiers = new List<StatModifier<double>>(modifiers);
            return result;
        }
    }

    public class StatFloat: Stat<float>
    {
        protected override float CaculatorValue()
        {
            modifiers.Sort(CompareModifier);

            var final = BaseValue;
            foreach (var modifier in modifiers)
            {
                if (modifier.type == StatModifierType.Add)
                    final += modifier.value;
                else if (modifier.type == StatModifierType.Mul)
                    final *= modifier.value;
            }
            return final;
        }

        public StatFloat Clone()
        {
            var result = (StatFloat)this.MemberwiseClone();
            result.modifiers = new List<StatModifier<float>>(modifiers);
            return result;
        }
    }

    public class StatInt: Stat<int>
    {
        protected override int CaculatorValue()
        {
            modifiers.Sort(CompareModifier);

            var final = BaseValue;
            foreach (var modifier in modifiers)
            {
                if (modifier.type == StatModifierType.Add)
                    final += modifier.value;
                else if (modifier.type == StatModifierType.Mul)
                    final *= modifier.value;
            }
            return final;
        }

        public StatInt Clone()
        {
            var result = (StatInt)this.MemberwiseClone();
            result.modifiers = new List<StatModifier<int>>(modifiers);
            return result;
        }
    }
}
