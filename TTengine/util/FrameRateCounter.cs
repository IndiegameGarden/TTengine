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
    public class FrameRateCounter: IScript
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

        /// <summary>
        /// Creates a new FrameRateCounter. TODO: screen position set.
        /// </summary>
        /// <returns></returns>
        public static Entity Create(Color textColor)
        {
            var e = TTFactory.CreateTextlet("##");
            e.GetComponent<PositionComp>().Position = new Vector2(2f, 2f);
            e.GetComponent<DrawComp>().DrawColor = textColor;
            var m = new ScriptComp(new FrameRateCounter(e.GetComponent<TextComp>()));
            e.AddComponent(m);
            e.Refresh();
            return e;
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
            string fps = string.Format("{0} fps [{1}]", frameRate, frameRateAvg );
            textComp.Text = fps;
        }

    }
}
