// (c) 2010-2015 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

﻿using System;
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
﻿using TTengine.Core;
﻿using TTengine.Comps;
﻿using TTengine.Modifiers;
using Artemis;

namespace TTengine.Util
{
    /// <summary>
    /// shows a framerate counter on screen (shows FPS) calculated
    /// from timing of draw/upd calls.
    /// </summary>
    public class FrameRateCounter: IScriptDraw
    {
        TextComp textComp;
        TimeSpan elapsedTime = TimeSpan.Zero;
        CountingTimer timer = new CountingTimer();

        /// <summary>
        /// Create a new FrameRateCounter script that modifies the text in the
        /// TextComp of the Entity, to show the FPS count.
        /// </summary>
        /// <param name="comp">The TextComp to modify</param>
        public FrameRateCounter(TextComp comp)
        {
            this.textComp = comp;
            timer.Start();
        }

        public void OnUpdate(ScriptContext ctx){
            timer.Update();
        }
        
        public void OnDraw(ScriptContext ctx)
        {
            timer.CountUp();
            int frameRateAvg = 0;
            if (ctx.SimTime > 0)
                frameRateAvg = (int)( (double)timer.CountTotal / timer.TimeTotal );
            string msg = string.Format("{0,4} fps [{1,4}] Tupd={2,3} Tdrw={2,3}", timer.Count, frameRateAvg, 
                Math.Round(1000.0 * TTGame.Instance.TimeUpdate),
                Math.Round(1000.0 * TTGame.Instance.TimeDraw) 
            );
            textComp.Text = msg;
        }

    }
}
