using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{
    /// <summary>
    /// A Modifier that generates a (tunable) cosine wave signal.
    /// Use Frequency, Amplitude and Offset to tune it.
    /// </summary>
    public class CosineModifier: Modifier
    {
        public double Frequency = 1;
        public double Amplitude = 1;
        public double Offset = 0;

        // TODO can we stop constructor proliferation!? solution: pass single objectToModify. Set code via a method.

        public CosineModifier(ModifyEntityDelegate code) :
            base(code)
        { }

        public CosineModifier(ModifyModifierDelegate code, Modifier modifierToModify):
            base(code,modifierToModify)
        { }

        public CosineModifier(ModifyCompDelegate code, Comp compToModify) :
            base(code, compToModify)
        { }

        protected override double GetValue(double time)
        {
            return Amplitude * Math.Cos(MathHelper.TwoPi * Frequency * time) + Offset;
        }
    }
}
