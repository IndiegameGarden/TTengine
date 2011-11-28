// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{

    /**
     * modifies a parameter periodically (sine-wave based)
     */
    public class SineModifier : Gamelet
    {
        float ampl, frequency, offset;
        ModifyAction action;

        public SineModifier(ModifyAction action, float ampl, float frequency, float offset)
            : base()
        {
            this.ampl = ampl;
            this.frequency = frequency;
            this.offset = offset;
            this.action = action;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            float val = offset + ampl * (float)Math.Sin(MathHelper.TwoPi * (double)frequency * SimTime);
            action(val);
        }
    }
}
