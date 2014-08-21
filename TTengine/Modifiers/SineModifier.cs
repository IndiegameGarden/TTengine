using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{
    /// <summary>
    /// A Modifier that generates a (tunable) sine wave signal.
    /// Use Frequency, Amplitude and Offset to tune it.
    /// </summary>
    public class SineModifier<T>: Modifier<T>
    {
        public double Frequency = 1;
        public double Amplitude = 1;
        public double Phase = 0;
        public double Offset = 0;

        public SineModifier(ModifierDelegate<T> code, T objectToModify):
            base(code,objectToModify)
        { }

        protected override double GetValue(double time)
        {
            return Amplitude * Math.Sin(MathHelper.TwoPi * Frequency * time + Phase) + Offset;
        }
    }
}
