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

        protected override void OnUpdate(ref UpdateParams myPars, ref UpdateParams parentPars)
        {
            base.OnUpdate(ref myPars, ref parentPars);
            float spd = 0.2f;
            Acceleration = new Vector2( spd * (float)Math.Sin(Math.PI * myPars.simTime), 
                                        spd * (float)Math.Cos(Math.PI * myPars.simTime)
                                      );
        }
    }
}
