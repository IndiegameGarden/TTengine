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
        int frameRate = 0;
        int frameCounter = 0;
        int frameCounterTotal = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        /// <summary>
        /// Create a new FrameRateCounter script that modifies the text in the
        /// TextComp of the Entity, to show the FPS count.
        /// </summary>
        /// <param name="comp">The TextComp to modify</param>
        public FrameRateCounter(TextComp comp)
        {
            this.textComp = comp;
        }

        public void OnUpdate(ScriptContext ctx){
            elapsedTime += TimeSpan.FromSeconds(ctx.Dt);

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }
        
        public void OnDraw(ScriptContext ctx)
        {
            frameCounter++;
            frameCounterTotal++;
            int frameRateAvg = 0;
            if (ctx.SimTime > 0)
                frameRateAvg = (int)(frameCounterTotal / ctx.SimTime);
            string fps = string.Format("{0,4} fps [{1,4}] Tupd={2,3}", frameRate, frameRateAvg, Math.Round(1000.0 * TTGame.Instance.TimeUpdate) );
            textComp.Text = fps;
        }

    }
}
