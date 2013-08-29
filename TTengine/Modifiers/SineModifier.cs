using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{
    public class SineModifier: Modifier
    {
        public double Frequency = 1;
        public double Amplitude = 1;
        public double Offset = 0;

        public SineModifier(ModifyEntityDelegate code):
            base(code)
        {
        }

        public SineModifier(ModifyModifierDelegate code, Modifier modifierToModify):
            base(code,modifierToModify)
        {
        }

        protected override double GetValue(double time)
        {
            return Amplitude * Math.Sin(MathHelper.TwoPi * Frequency * time) + Offset;
        }
    }
}
