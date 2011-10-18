// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine;

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

            float z = 17f - 15f * (float) Math.Sin( MathHelper.TwoPi * 0.03324 * (float)p.simTime );
            effect.Parameters["Zoom"].SetValue(z);
            effect.Parameters["Time"].SetValue((float) p.gameTime.TotalGameTime.TotalSeconds);
        }
        
    }
}
