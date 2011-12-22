// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{

    /**
     * modifies a parameter periodically (custom function based)
     */
    public class MyFuncyModifier : Gamelet
    {
        ModifyAction action;

        public MyFuncyModifier(ModifyAction action)
            : base()
        {
            this.action = action;
        }

        public MyFuncyModifier(ModifyAction action, float startTime)
            : base()
        {
            this.StartTime = startTime;
            this.action = action;
        }

        public MyFuncyModifier(ModifyAction action, float startTime, float duration)
            : base()
        {
            this.Duration = duration;
            this.StartTime = startTime;
            this.action = action;
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            action(SimTime);
        }
    }
}
