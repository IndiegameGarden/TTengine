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
            BackgroundColor = Color.DarkSlateGray;
        }

        public override void Create()
        {
            var ch = TTFactory.BuildChannel;
            TTFactory.BuildTo(ch);

            // put some content in the main channel
            var t0 = new TestRelativeMotion();
            t0.Create();

            // create an additional child channel that renders onto the main channel
            // content for 1st screen: call upon another unit test
            var t1 = new TestAnimatedSprite(); 
            var ch1 = TTFactory.CreateChannel(320, 320, t1.BackgroundColor, true);
            TTFactory.BuildTo(ch1);
            t1.Create();
            ch.AddChild(ch1);

            // second child channel
            var t2 = new TestRelativeMotion(); 
            var ch2 = TTFactory.CreateChannel(400, 320, t2.BackgroundColor, true);
            TTFactory.BuildTo(ch2);            
            t2.Create();
            ch.AddChild(ch2);

            // main channel: shows the child channels as sprites
            TTFactory.BuildTo(ch);
            var scr1 = TTFactory.CreateSpritelet(ch1);
            scr1.GetComponent<PositionComp>().Position = new Vector3(350f, 350f,0.1f);
            scr1.GetComponent<VelocityComp>().Velocity2D = new Vector2(5f, 0.5f);

            var scr2 = TTFactory.CreateSpritelet(ch2);
            scr2.GetComponent<PositionComp>().Position = new Vector3(650f, 250f, 0.1f);
        }

    }
}
