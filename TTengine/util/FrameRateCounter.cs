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
    /// from timing of draw/upd calls; and more profiling info if available.
    /// </summary>
    public class FrameRateCounter: IScript
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

        public void OnUpdate(ScriptComp ctx){
            timer.Update();
        }
        
        public void OnDraw(ScriptComp ctx)
        {
            timer.CountUp();
            int frameRateAvg = 0;
            double timeTotal = ctx.Entity.C<ScriptComp>().SimTime;

            if (timer.TimeTotal > 0)
                frameRateAvg = (int)( (double)timer.CountTotal / timeTotal );
            string msg;
            if (TTGame.Instance.IsProfiling)
                msg = string.Format("{0,5:D4} FPS [{1,5:D4}] Tupd={2,5:N1}ms Tdrw={3,5:N1}ms", timer.Count, frameRateAvg,
                    Math.Round(1000.0 * TTGame.Instance.ProfilingTimerUpdate.TimePerCount),
                    Math.Round(1000.0 * TTGame.Instance.ProfilingTimerDraw.TimePerCount) 
                );
            else
                msg = string.Format("{0,5} FPS [{1,5}]", timer.Count, frameRateAvg);
            textComp.Text = msg;
        }

    }
}
