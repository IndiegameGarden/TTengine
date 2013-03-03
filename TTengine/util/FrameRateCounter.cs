// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

﻿using System;
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
﻿using TTengine.Core;

namespace TTengine.Util
{
    /**
     * shows a framerate counter on screen (FPS) calculated
     * from timing of draw/upd calls.
     */
    public class FrameRateCounter : Gamelet
    {
        SpriteFont spriteFont;
        int frameRate = 0;
        int frameCounter = 0;
        int frameCounterTotal = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FrameRateCounter(): this(0.5f, 0.01f)
        {
            //
        }

        public FrameRateCounter(float x, float y)
        {         
            DrawC.LayerDepth = 0.0f;
            Motion.Position = new Vector2(x, y);
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>(@"TTFrameRateCounter");
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            elapsedTime += TimeSpan.FromSeconds(p.Dt);

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        protected override void OnDraw(ref DrawParams p)
        {
            frameCounter++;
            frameCounterTotal++;
            int frameRateAvg = 0;
            if (SimTime > 0f)
                frameRateAvg = (int)(frameCounterTotal / SimTime);
            string fps = string.Format("{0} fps [{1}]", frameRate, frameRateAvg );
            Vector2 pos = Motion.PositionAbsZoomedPixels;
            DrawC.MySpriteBatch.DrawString(spriteFont, fps, pos,
                        Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, DrawC.LayerDepth - 0.00001f);
            DrawC.MySpriteBatch.DrawString(spriteFont, fps, pos,
                        DrawC.DrawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, DrawC.LayerDepth);
        }
    }
}
