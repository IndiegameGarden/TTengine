// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        public GameFactory Factory;
        Channel titleChannel, gameChannel;

        public Game1()
        {
            GraphicsMgr.IsFullScreen = false;
            GraphicsMgr.PreferredBackBufferWidth = 1024; 
            GraphicsMgr.PreferredBackBufferHeight = 768;
            IsMusicEngine = false;
        }

        protected override void Initialize()
        {
            Factory = GameFactory.Instance;
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Space))
            {
                ChannelMgr.ZapTo(titleChannel);
            }
            else
            {
                ChannelMgr.ZapTo(gameChannel);
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // title channel
            titleChannel = ChannelMgr.CreateChannel();
            ChannelMgr.ZapTo(titleChannel); // TODO function to create on it without seeing it.
            ActiveScreen.GetComponent<ScreenComp>().BackgroundColor = Color.Black;

            // add framerate counter
            FrameRateCounter.Create(Color.White);

            var t = Factory.CreateMovingTextlet(new Vector2(0.5f, 0.5f), "Title Screen");
            t.GetComponent<DrawComp>().DrawColor = Color.LightGoldenrodYellow;
            t.GetComponent<ScaleComp>().Scale = 4;


            // game channel
            gameChannel = ChannelMgr.CreateChannel();
            ChannelMgr.ZapTo(gameChannel); 
            ActiveScreen.GetComponent<ScreenComp>().BackgroundColor = Color.White;

            // add framerate counter
            FrameRateCounter.Create(Color.Black);

            // add several sprites             
            for (float x = 0.1f; x < 1.6f; x += 0.1f)
            {
                for (float y = 0.1f; y < 1f; y += 0.1f)
                {
                    Factory.CreateHyperActiveBall(new Vector2(x,y));
                    Factory.CreateMovingTextlet(new Vector2(x,y),"This is the\nTTengine test. !@#$1234");
                    //break;
                }
                //break;
            }
        }       

    }
}
