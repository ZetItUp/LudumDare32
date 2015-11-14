using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APEngine.Entities
{
    public class GameAttribute
    {
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
        public int Value { get; private set; }

        public GameAttribute(int min, int max)
        {
            MinValue = min;
            MaxValue = max;
            Value = MaxValue;
        }

        public void Subtract(int amount)
        {
            Value -= amount;

            if (Value < MinValue)
                Value = MinValue;
        }

        public void Add(int amount)
        {
            Value += amount;

            if (Value > MaxValue)
                Value = MaxValue;
        }
    }
}
