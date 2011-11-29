// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{

    /**
     * modifies a parameter periodically (sine-wave based)
     */
    public class MyFuncyModifier : Gamelet
    {
        ModifyAction action;

        public MyFuncyModifier(ModifyAction action)
            : base()
        {
            this.action = action;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            action(SimTime);
        }
    }
}
