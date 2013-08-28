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
using TTengine.Behaviors;
using TTengine.Util;

using Artemis;
using TreeSharp;

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
            GraphicsMgr.PreferredBackBufferWidth = 1024; 
            GraphicsMgr.PreferredBackBufferHeight = 768;
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

            // add several sprites             
            for (float x = 0.1f; x < 1.6f; x += 0.20f)
            {
                for (float y = 0.1f; y < 1f; y += 0.1f)
                {
                    var ball = Factory.CreateHyperActiveBall(new Vector2(x,y));
                    //break;
                }
                //break;
            }

        }

    }
}
