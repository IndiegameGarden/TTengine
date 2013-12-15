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
            TTFactory.RenderTo(null); // select the default render screen (from channel)
            var scr1 = TTFactory.CreateScreenlet( 320, 320 );
            scr1.GetComponent<DrawComp>().DrawPosition = new Vector2(50f, 50f);
            // content for 1st screen: call upon another unit test
            TTFactory.RenderTo(scr1);
            var t = new TestRelativeMotion();
            t.Initialize(TestFactory.Instance);
            t.Create();           

            TTFactory.RenderTo(null); // reset back to default render screen (from channel)
            var scr2 = TTFactory.CreateScreenlet(320, 320);
            scr2.GetComponent<DrawComp>().DrawPosition = new Vector2(420f, 20f);
            TTFactory.RenderTo(scr2);
            var t2 = new TestAnimatedSprite();
            t2.Initialize(TestFactory.Instance);
            t2.Create();
            //scr2.GetComponent<ScreenComp>().Visible = false;

            TTFactory.RenderTo(null); 
        }

    }
}
