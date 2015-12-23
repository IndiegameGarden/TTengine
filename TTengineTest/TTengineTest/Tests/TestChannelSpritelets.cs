using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Tests basic Channel Spritelets. These are channels (virtual screens) rendered
    /// as texture onto a sprite.</summary>
    class TestChannelSpritelets : Test
    {

        public TestChannelSpritelets()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var ch = TTFactory.BuildScreen;

            // put some content in the main channel
            TTFactory.BuildScreen.BackgroundColor = Color.White;
            var t0 = new TestScaling();
            t0.Create();

            // create an additional child channel that renders onto the main channel
            // content for 1st screen: call upon another unit test
            var ch1 = TTFactory.CreateChannel(Color.LightSalmon, true, 200, 400);
            TTFactory.BuildTo(ch1);
            var t1 = new TestRelativeMotion();
            t1.Create();

            // second child channel
            var ch2 = TTFactory.CreateChannel(Color.LightSeaGreen, true, 200, 400);
            TTFactory.BuildTo(ch2);
            var t2 = new TestRelativeMotion();
            t2.Create();

            // 3rd
            var ch3 = TTFactory.CreateChannel(Color.LightPink, true, 200, 400);
            TTFactory.BuildTo(ch3);
            var t3 = new TestRelativeMotion();
            t3.Create();

            // 4th
            var ch4 = TTFactory.CreateChannel(Color.LightGreen, true, 700, 100);
            TTFactory.BuildTo(ch4);
            var t4 = new TestZoomedChannel();
            t4.Create();

            // main channel: shows the child channels as sprites
            TTFactory.BuildTo(ch);
            var scr1 = TTFactory.CreateSpritelet(ch1);
            scr1.GetComponent<PositionComp>().Position = new Vector2(50f, 50f);
            //scr1.GetComponent<VelocityComp>().Velocity2D = new Vector2(5f, 0.5f);

            var scr2 = TTFactory.CreateSpritelet(ch2);
            scr2.GetComponent<PositionComp>().Position = new Vector2(300f, 50f);

            var scr3 = TTFactory.CreateSpritelet(ch3);
            scr3.GetComponent<PositionComp>().Position = new Vector2(550f, 50f);

            var scr4 = TTFactory.CreateSpritelet(ch4);
            scr4.GetComponent<PositionComp>().Position = new Vector2(50f, 500f);
        }

    }
}
