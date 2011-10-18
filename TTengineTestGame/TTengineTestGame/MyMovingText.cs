// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine;

namespace TTengineTestGame
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
            Acceleration = new Vector2( spd * (float)Math.Sin(Math.PI * p.simTime), 
                                        spd * (float)Math.Cos(Math.PI * p.simTime)
                                      );
        }
    }
}
