// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
﻿using Microsoft.Xna.Framework;
﻿using TTengine.Core;

namespace Game1
{
    public class MyMovingTextlet: MyTextlet
    {
        public MyMovingTextlet(string text)
            : base(text)
        {
            //
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            float spd = 0.2f;
            Motion.Acceleration = new Vector2( spd * (float)Math.Sin(Math.PI * p.SimTime), 
                                        spd * (float)Math.Cos(Math.PI * p.SimTime)
                                      );
        }
    }
}
