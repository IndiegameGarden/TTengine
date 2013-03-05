// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
﻿using Microsoft.Xna.Framework;
﻿using TTengine.Core;

namespace TTengineTestGame
{
    public class HypnoEfflet: Efflet
    {
        public HypnoEfflet()
            : base("Hypno")
        {
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            float z = 17f - 15f * (float) Math.Sin( MathHelper.TwoPi * 0.03324 * p.SimTime );
            effect.Parameters["Zoom"].SetValue(z);
            effect.Parameters["Time"].SetValue(p.SimTime);
        }
        
    }
}
