// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TTengine;

namespace TTengine.util
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

        public FrameRateCounter()
            : base()
        {
            Position = new Vector2(0.5f, 0.01f);
        }

        public FrameRateCounter(float x, float y)
            : base()
        {
            Position = new Vector2(x, y);
        }


        protected override void OnInit()
        {
            LayerDepth = 0.0f;
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>(@"TTFrameRateCounter");
        }


        protected override void OnUpdate(ref UpdateParams p)
        {
            elapsedTime += TimeSpan.FromSeconds(p.dt);

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
            Screen.spriteBatch.DrawString(spriteFont, fps, Screen.ToPixels(Position), 
                Color.Black, 0f, Vector2.Zero,1f,SpriteEffects.None,LayerDepth-0.00001f);
            Screen.spriteBatch.DrawString(spriteFont, fps, Screen.ToPixels(Position), 
                drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepth);
        }
    }
}
