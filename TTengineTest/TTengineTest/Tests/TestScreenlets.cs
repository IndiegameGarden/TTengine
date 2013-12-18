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
            // put some content in the main channel
            var t0 = new TestRelativeMotion();
            t0.Create();

            // create an additional child channel that renders onto the main channel
            // content for 1st screen: call upon another unit test
            var ch1 = TTFactory.CreateChannel(320, 320);
            var t1 = new TestRelativeMotion();
            t1.Create();

            /*
            TTFactory.RenderTo(null); // reset back to default render screen (from channel)
            var scr2 = TTFactory.CreateScreenlet(320, 320);
            scr2.GetComponent<DrawComp>().DrawPosition = new Vector2(520f, 20f);
            TTFactory.RenderTo(scr2);
            var t2 = new TestAnimatedSprite();
            t2.Initialize(TestFactory.Instance);
            t2.Create();
            //scr2.GetComponent<ScreenComp>().Visible = false;
            */
            TTFactory.RenderTo(null);

            var scr1 = TTFactory.CreateSpritelet(ch1);
            scr1.GetComponent<DrawComp>().DrawPosition = new Vector2(150f, 50f);

        }

    }
}
