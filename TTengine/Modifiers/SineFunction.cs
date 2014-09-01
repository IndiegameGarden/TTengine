using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{
    /// <summary>
    /// A Function that generates a (tunable) sine wave.
    /// Use Frequency, Amplitude and Offset to tune it.
    /// </summary>
    public class SineFunction: Function
    {
        public double Frequency = 1;
        public double Amplitude = 1;
        public double Phase = 0;
        public double Offset = 0;

        public override double Value(double inp)
        {
            inp = base.Value(inp);
            return Amplitude * Math.Sin(MathHelper.TwoPi * Frequency * inp + Phase) + Offset;
        }
    }
}
