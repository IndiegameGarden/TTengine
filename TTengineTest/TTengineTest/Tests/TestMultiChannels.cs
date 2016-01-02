using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Tests multiple Channels. Each Channel is later rendered as a sprite onto the main screen.
    /// </summary>
    class TestMultiChannels : Test
    {
        public override void Create()
        {
            // put some content in the main channel
            var t0 = new TestScaling();
            t0.Create();

            // create an additional child channel
            // content for 1st screen: call upon another unit test
            var ch1 = TestFactory.CreateChannel(Color.LightSalmon, true, 200, 400);
            ch1.GetComponent<WorldComp>().TimeWarp = 0.1;
            BuildTo(ch1);
            var t1 = new TestRelativeMotion();
            t1.Create();

            // second child channel
            BuildToDefault();
            var ch2 = TestFactory.CreateChannel(Color.LightSeaGreen, true, 200, 400);
            ch1.GetComponent<WorldComp>().TimeWarp = 0.5;
            BuildTo(ch2);
            var t2 = new TestRelativeMotion();
            t2.Create();

            // 3rd
            BuildToDefault();
            var ch3 = TestFactory.CreateChannel(Color.LightPink, true, 200, 400);
            ch1.GetComponent<WorldComp>().TimeWarp = 2.0;
            BuildTo(ch3);
            var t3 = new TestRelativeMotion();
            t3.Create();

            // 4th
            BuildToDefault();
            var ch4 = TestFactory.CreateChannel(Color.LightGreen, true, 700, 100);
            ch1.GetComponent<WorldComp>().TimeWarp = 0.0; // Paused
            BuildTo(ch4);
            var t4 = new TestRelativeMotion();
            t4.Channel = Channel;
            t4.Create();

            // main channel: shows the child channels as sprites on different positions
            BuildToDefault();
            var scr1 = TestFactory.CreateSpritelet(ch1);
            scr1.GetComponent<PositionComp>().Position = new Vector2(50f, 50f);

            var scr2 = TestFactory.CreateSpritelet(ch2);
            scr2.GetComponent<PositionComp>().Position = new Vector2(300f, 50f);

            var scr3 = TestFactory.CreateSpritelet(ch3);
            scr3.GetComponent<PositionComp>().Position = new Vector2(550f, 50f);

            var scr4 = TestFactory.CreateSpritelet(ch4);
            scr4.GetComponent<PositionComp>().Position = new Vector2(50f, 500f);
        }

    }
}
