// (c) 2010-2012 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{

    /**
     * modifies a parameter according to a sine-wave function
     */
    public class SineModifier : Modifier
    {
        float ampl, frequency, offset;
        ValueModifier action;

        public SineModifier(ValueModifier action, float ampl, float frequency)
            : base()
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = 0f;
            this.action = action;
        }
        
        public SineModifier(ValueModifier action, float ampl, float frequency, float offset)
            : base()
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = offset;
            this.action = action;
        }

        protected override float ModifierValue(ref UpdateParams p)
        {
            float val = offset + ampl * (float)Math.Sin(MathHelper.TwoPi * (double)frequency * SimTime);
            return val;
        }
    }
}
