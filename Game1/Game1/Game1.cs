// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//using TTengine.Behaviors;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;

using Artemis;

using Game1.Factories;

namespace Game1
{
    /// <summary>
    /// Main game class, using TTGame template
    /// </summary>
    public class Game1 : TTGame
    {
        public Game1()
        {
            GraphicsMgr.IsFullScreen = false;
            GraphicsMgr.PreferredBackBufferWidth = 800; 
            GraphicsMgr.PreferredBackBufferHeight = 600;
            IsMusicEngine = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ActiveScreen.BackgroundColor = Color.White;

            /*
            // add a static text
            MyTextlet txt = new MyTextlet("TTengine basic test!");
            txt.Motion.Position = new Vector2(0.01f, 0.4f);
            txt.DrawC.DrawColor = Color.White;
            Screen.Add(txt);
            */

            // add several sprites and set some specific velocity per item
            Random rnd = new Random();
            for (float j = 0.1f; j < 1.6f; j += 0.20f)
            {
                for (float i = 0.1f; i < 1f; i += 0.1f)
                {
                    var ball = Factory.CreateBall(0.04f + 0.06f * (float)rnd.NextDouble());

                    // position and velocity set
                    ball.GetComponent<PositionComp>().Position = new Vector2(j, i);
                    ball.GetComponent<VelocityComp>().Velocity = 0.2f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);
                    //ball.Motion.Rotate = (float)(Math.PI * 2 * rnd.NextDouble());                    
                    //ball.Timing.StartTime = 10f * (float)rnd.NextDouble();

                    // duration of entity
                    ball.AddComponent(new ExpiresComp(1000f + 5f * (float)rnd.NextDouble()));

                    // blink
                    //var ai = new AIComp();
                    ball.AddComponent(new BlinkComp(0.3+5*rnd.NextDouble(),0.4+0.4*rnd.NextDouble()));
                    //ball.AddComponent(ai);

                    ball.Refresh();
                    //break;
                }
                //break;
            }

        }

    }
}