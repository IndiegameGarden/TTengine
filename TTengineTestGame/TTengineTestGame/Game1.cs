// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TTengine;
using TTengine.Core;
using TTengine.Util;

namespace TTengineTestGame
{
    public class Game1 : TTGame
    {
        protected override void InitTTGame()
        {
            MyWindowWidth = 640;
            MyWindowHeight = 480;
        }

        protected override void InitTTContent()
        {
            Screen.DrawC.DrawColor = Color.White;

            // add a static text 'MyTextlet'
            MyTextlet txt = new MyTextlet("TTengine shader test using Efflet");
            txt.Motion.Position = new Vector2(0.01f, 0.4f);
            txt.DrawC.DrawColor = Color.Black;
            Screen.Add(txt);

            // add several Spritelets and set some specific velocity per item
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

                    Screen.Add(ball);
                }
            }

            HypnoEfflet eff = new HypnoEfflet();
            eff.DrawC.Alpha = 0.84f;
            Screen.Add(eff);

        }

        protected override void LoadTTContent()
        {
        }

    }
}
