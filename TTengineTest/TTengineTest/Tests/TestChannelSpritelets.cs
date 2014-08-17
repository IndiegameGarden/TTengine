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
            var ch = TTFactory.BuildChannel;
            TTFactory.BuildTo(ch);

            // put some content in the main channel
            var t0 = new TestScaling();
            ch.Screen.BackgroundColor = Color.White;
            t0.Create();

            // create an additional child channel that renders onto the main channel
            // content for 1st screen: call upon another unit test
            var t1 = new TestRelativeMotion(); 
            var ch1 = TTFactory.CreateChannel(200, 400, Color.LightSalmon, true);
            TTFactory.BuildTo(ch1);
            t1.Create();
            ch.AddChild(ch1);

            // second child channel
            var t2 = new TestRelativeMotion(); 
            var ch2 = TTFactory.CreateChannel(200, 400, Color.LightSeaGreen, true);
            TTFactory.BuildTo(ch2);            
            t2.Create();
            ch.AddChild(ch2);

            // 3rd
            var t3 = new TestRelativeMotion();
            var ch3 = TTFactory.CreateChannel(200, 400, Color.LightPink, true);
            TTFactory.BuildTo(ch3);
            t3.Create();
            ch.AddChild(ch3);

            // 4th
            var t4 = new TestZoomedChannel();
            var ch4 = TTFactory.CreateChannel(700, 100, Color.LightGreen, true);
            TTFactory.BuildTo(ch4);
            t4.Create();
            ch.AddChild(ch4);

            // main channel: shows the child channels as sprites
            TTFactory.BuildTo(ch);
            var scr1 = TTFactory.CreateSpritelet(ch1);
            scr1.GetComponent<PositionComp>().Position = new Vector3(50f, 50f,0.1f);
            //scr1.GetComponent<VelocityComp>().Velocity2D = new Vector2(5f, 0.5f);

            var scr2 = TTFactory.CreateSpritelet(ch2);
            scr2.GetComponent<PositionComp>().Position = new Vector3(300f, 50f, 0.1f);

            var scr3 = TTFactory.CreateSpritelet(ch3);
            scr3.GetComponent<PositionComp>().Position = new Vector3(550f, 50f, 0.1f);

            var scr4 = TTFactory.CreateSpritelet(ch4);
            scr4.GetComponent<PositionComp>().Position = new Vector3(50f, 500f, 0.1f);
        }

    }
}
