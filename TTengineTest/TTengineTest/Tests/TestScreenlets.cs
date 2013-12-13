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
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var scr1 = TTFactory.CreateScreenlet( TTGame.Instance.BuildWorld, 320, 200);
            scr1.GetComponent<DrawComp>().DrawPosition = new Vector2(20f, 20f);
            // content for 1st screen: put in another unit test
            TTFactory.BuildTo(scr1);
            var t = new TestRelativeMotion();
            t.Create();

            TTFactory.BuildTo(null); // reset back to default build screen (from channel)
            var scr2 = TTFactory.CreateScreenlet(TTGame.Instance.BuildWorld, 320, 200);
            scr2.GetComponent<DrawComp>().DrawPosition = new Vector2(360f, 20f);
            TTFactory.BuildTo(scr2);
            var t2 = new TestAnimatedSprite();
            t2.Create();

        }

    }
}
