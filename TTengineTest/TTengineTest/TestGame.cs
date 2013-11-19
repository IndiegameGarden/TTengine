using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;
using TTengine.Util;

using Artemis;
using Artemis.Interface;
using TreeSharp;


namespace TTengineTest
{
    /// <summary>
    /// Visual "unit" tests of various aspects of the TTengine. Press keys to cycle through tests.
    /// </summary>
    public class TestGame : TTGame
    {
        public TestFactory Factory;
        KeyboardState kbOld = Keyboard.GetState();
        int channel = 0;
        List<Channel> channels = new List<Channel>();

        public TestGame()
        {
            GraphicsMgr.IsFullScreen = false;
            GraphicsMgr.PreferredBackBufferWidth = 1024; 
            GraphicsMgr.PreferredBackBufferHeight = 700;
            IsAudio = true;
        }

        protected override void Initialize()
        {
            Factory = TestFactory.Instance;
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape) && !kbOld.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            
            if ((kb.IsKeyDown(Keys.Space) && !kbOld.IsKeyDown(Keys.Space)) ||
                (kb.IsKeyDown(Keys.Right) && !kbOld.IsKeyDown(Keys.Right)) )
            {
                if (channel < channels.Count-1)
                {
                    channel++;
                    ChannelMgr.ZapTo(channels[channel]);
                }
            }
            else if (kb.IsKeyDown(Keys.Left) && !kbOld.IsKeyDown(Keys.Left))
            {
                if (channel > 0)
                {
                    channel--;
                    ChannelMgr.ZapTo(channels[channel]);
                }
            }
            else if (kb.IsKeyDown(Keys.Escape) && !kbOld.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            kbOld = kb;
        }

        private void DoTest(Test t)
        {
            var c = ChannelMgr.CreateChannel();
            c.Screen.GetComponent<ScreenComp>().BackgroundColor = t.BackgroundColor;

            channels.Add(c);
            t.Initialize(Factory);
            t.Create();

            // add framerate counter
            var col = TTutil.InvertColor(t.BackgroundColor);
            FrameRateCounter.Create(col);

            Factory.CreateTextlet(new Vector2(2f, GraphicsMgr.PreferredBackBufferHeight-20f), t.GetType().Name, col);

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Here all the tests are listed
            DoTest(new TestAnimatedSprite());
            //DoTest(new TestScaling());
            DoTest(new TestSpriteField());
            DoTest(new TestAudioBasics());
            DoTest(new TestContentLoad());
            DoTest(new TestRelativeMotion());
            DoTest(new TestLinearMotion());
            DoTest(new TestRotation());

            ChannelMgr.ZapTo(channels[0]);

        }       

    }

}
