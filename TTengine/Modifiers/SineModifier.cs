// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
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

        public SineModifier(String property, float ampl, float frequency)
            : base(property)
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = 0f;
        }
        
        public SineModifier(String property, float ampl, float frequency, float offset)
            : base(property)
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = offset;
        }

        public SineModifier(String property, String subProperty, float ampl, float frequency)
            : base(property,subProperty)
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = 0f;
        }

        public SineModifier(String property, String subProperty, float ampl, float frequency, float offset)
            : base(property,subProperty)
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = offset;
        }

        // calc sine modifier values
        protected override float ModifierValue(ref UpdateParams p)
        {
            float val = offset + ampl * (float)Math.Sin(MathHelper.TwoPi * (double)frequency * SimTime);
            return val;
        }
    }
}
