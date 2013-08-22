// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Util;

namespace Game1
{
    /// <summary>
    /// Main game class
    /// </summary>
    public class Game1 : TTGame
    {
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferHeight = 640;
            Graphics.PreferredBackBufferWidth = 480;
            this.IsMusicEngine = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            /*
            // add a static text
            MyTextlet txt = new MyTextlet("TTengine basic test!");
            txt.Motion.Position = new Vector2(0.01f, 0.4f);
            txt.DrawC.DrawColor = Color.White;
            Screen.Add(txt);
            */

            // add several sprites and set some specific velocity per item
            Random rnd = new Random();
            Gamelet ball = null;
            for (float j = 0.1f; j < 1.6f; j += 0.20f)
            {
                for (float i = 0.1f; i < 1.0f; i += 0.1f)
                {
                    //Spritelet ball = new Spritelet("ball");
                    ball = new Ball();
                    ball.Motion.Position = new Vector2(j, i); //(float) rnd.NextDouble() , (float) rnd.NextDouble() );
                    ball.Motion.Velocity = 0.1f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);
                    ball.Motion.Rotate = (float)(Math.PI * 2 * rnd.NextDouble());
                    ball.Motion.Scale = 0.4f + 0.6f * (float)rnd.NextDouble();
                    
                    ball.Timing.StartTime = 10f * (float)rnd.NextDouble();
                    ball.Timing.Duration = 10f + 5f * (float)rnd.NextDouble();

                    Screen.Add(ball);
                }
            }

        }

    }
}
