using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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
        List<Entity> testChannels = new List<Entity>();

        public TestGame()
        {
            IsAudio = true;
            IsProfiling = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Here all the tests are created
            //DoTest(new TestPostEffects()); //FIXME
            DoTest(new TestRelativeMotion());
            DoTest(new TestMultiChannels());
            DoTest(new TestGamepad());
            DoTest(new TestModifiers());
            DoTest(new TestZoomedScreenlet());
            DoTest(new TestAudioBasics()); //FIXME?
            DoTest(new TestContentLoad());
            DoTest(new TestTargetMotion());
            DoTest(new TestScaling());            
            DoTest(new TestTextureSamplingShader());
            DoTest(new TestBTAI());
            DoTest(new TestSphereCollision());
            DoTest(new TestAnimatedSprite());
            DoTest(new TestBasicShader());
            DoTest(new TestMixedShaders()); 
            DoTest(new TestLinearMotion());
            DoTest(new TestRotation());
            DoTest(new TestSpritePixelGetSet());            

            // pick the initial one and activate it
            ZapChannel(0);

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
                (kb.IsKeyDown(Keys.PageDown) && !kbOld.IsKeyDown(Keys.PageDown)) )
            {
                ZapChannel(+1);
            }
            else if (kb.IsKeyDown(Keys.PageUp) && !kbOld.IsKeyDown(Keys.PageUp))
            {
                ZapChannel(-1);
            }
            kbOld = kb;
        }

        private void ZapChannel(int delta)
        {
            int nch = channel + delta;
            if (nch < 0)
                nch += testChannels.Count;
            if (nch >= testChannels.Count)
                nch -= testChannels.Count;
            if (channel != nch)
            {
                testChannels[channel].IsEnabled = false;
                testChannels[channel].Refresh();
            }
            testChannels[nch].IsEnabled = true;
            testChannels[nch].Refresh();
            channel = nch;
        }

        private void DoTest(Test test)
        {
            var ch = TestFactory.CreateChannel(test.BackgroundColor);
            test.Channel = ch;
            testChannels.Add(ch);
            test.BuildToDefault(); // build test to the new channel
            test.Create();

            // add framerate counter
            test.BuildToDefault();
            var col = TTUtil.InvertColor(test.BackgroundColor);
            TestFactory.CreateFrameRateCounter(col);

            // add test info as text
            Factory.CreateTextlet(new Vector2(2f, GraphicsMgr.PreferredBackBufferHeight-20f), test.GetType().Name, col);

            // disable the new channel by default
            ch.IsEnabled = false;
            ch.Refresh();

            // ensure new channels are built to main Channel again
            TestFactory.BuildTo(MainChannel);
        }

    }

}
