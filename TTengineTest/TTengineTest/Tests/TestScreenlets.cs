using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Tests basic Screenlet functions (multiple virtual screens)</summary>
    class TestScreenlets : Test
    {

        public TestScreenlets()
            : base()        
        {
            BackgroundColor = Color.DarkSlateGray;
        }

        public override void Create()
        {
            //var ch = TTFactory.BuildChannel;
            //TTFactory.BuildTo(ch);

            // put some content in the main channel
            var t0 = new TestRelativeMotion();
            t0.Create();

            // create an additional child channel that renders onto the main channel
            // content for 1st screen: call upon another unit test
            //var ch1 = TTFactory.CreateChannel(320, 320);
            //var t1 = new TestRelativeMotion();
            //t1.Create();
            //ch.AddChild(ch1);

            /*
            // second child channel
            var ch2 = TTFactory.CreateChannel(400, 320);
            var t2 = new TestAnimatedSprite();
            t2.Create();
            ch.AddChild(ch2);
            */

            // main channel: shows the child channels as sprites
            //TTFactory.BuildTo(mainChannel);
            //var scr1 = TTFactory.CreateSpritelet(ch1);
            //scr1.GetComponent<DrawComp>().DrawPosition = new Vector2(150f, 50f);

            //var scr2 = TTFactory.CreateSpritelet(ch2);
            //scr2.GetComponent<DrawComp>().DrawPosition = new Vector2(540f, 50f);
        }

    }
}
