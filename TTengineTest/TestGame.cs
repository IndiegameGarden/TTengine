// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;

using Artemis;
using Artemis.Utils;


namespace TTengineTest
{
    /// <summary>
    /// Visual "unit" tests of various aspects of the TTengine. Press keys to cycle through tests.
    /// </summary>
    public class TestGame : TTGame
    {
        public static TestFactory Factory;
        KeyboardState kbOld = Keyboard.GetState();
        int channel = 0;
        List<Test> tests = new List<Test>();
        Entity textOverlayChannel;

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
            DoTest(new TestGeom3D());
            DoTest(new TestFxSprite());
            DoTest(new TestFxSprite2());
            DoTest(new TestCrtEffect());
            DoTest(new TestMixedShaders());            
            DoTest(new TestAnimatedSprite());
            DoTest(new TestTextureSamplingShader());
            DoTest(new TestBasicShader());
            DoTest(new TestLinearMotion());
            DoTest(new TestRotation());
            DoTest(new TestModifiers());
            DoTest(new TestScriptThreadedSystemForBuilding());
            DoTest(new TestTransparentChannels());
            DoTest(new TestRelativeMotion());
            DoTest(new TestMultiChannels());
            DoTest(new TestGamepad());
            DoTest(new TestZoomedScreen());
            DoTest(new TestAudioBasics()); //FIXME? audio plays too soon
            DoTest(new TestContentLoad());
            DoTest(new TestTargetMotion());
            DoTest(new TestScaling());            
            DoTest(new TestBTAI());
            DoTest(new TestSphereCollision());
            DoTest(new TestSpritePixelGetSet());

            // create the text overlay channel
            this.textOverlayChannel = CreateTextOverlayChannel();

            // pick the initial one and activate it
            ZapChannel(0);

        }

        protected override void UnloadContent()
        {
            foreach(Test t in tests)
            {
                t.Channel.C<WorldComp>().World.UnloadContent();
            }
            base.UnloadContent();
        }

        protected override void Initialize()
        {
            Factory = new TestFactory();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape) && !kbOld.IsKeyDown(Keys.Escape))
            {
                UnloadContent();
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
                nch += tests.Count;
            if (nch >= tests.Count)
                nch -= tests.Count;
            if (channel != nch)
            {
                tests[channel].Channel.IsEnabled = false;
                tests[channel].Channel.Refresh();
            }
            tests[nch].Channel.IsEnabled = true;
            tests[nch].Channel.Refresh();
            channel = nch;

            // update text overlays with font color of the test
            // TBD
            Bag<Entity> t = this.textOverlayChannel.C<WorldComp>().World.EntityManager.GetEntities(Aspect.All(new Type[]{ typeof(TextComp) }));
            foreach (Entity e in t)
            {
                e.C<DrawComp>().DrawColor = tests[channel].FontColor;
                e.C<TextComp>().Text = tests[channel].GetType().Name;
            }

        }

        private Entity CreateTextOverlayChannel()
        {
            Factory.BuildToRoot();
            var ch = Factory.CreateChannel(Factory.New(), Color.Transparent);
            Factory.BuildTo(ch);

            // create framerate counter stats
            Factory.CreateFrameRateCounter(Factory.New(), Color.White, 20);

            // add test info as text
            Factory.CreateText(Factory.New(), new Vector2(2f, GraphicsMgr.PreferredBackBufferHeight - 40f), "TestGame", Color.White, 0f);

            return ch;
        }

        private void DoTest(Test test)
        {
            Factory.BuildToRoot();

            var ch = Factory.NewDisabled(); // a channel is disabled by default - only one turned on later.
            Factory.CreateChannel(ch, test.BackgroundColor);
            test.FontColor = TTUtil.InvertColor(test.BackgroundColor);

            test.Channel = ch;
            test.BuildTo(ch); // build test to the new channel (test.Channel)
            test.BuildAll();  // create all the test's content
            tests.Add(test);

        }

    }

}
