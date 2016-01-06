using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary></summary>
    class TestPostEffects : Test
    {

        public override void Create()
        {
            // create an additional child channel that renders onto the main channel
            // content for 1st screen: call upon another unit test
            var t1 = new TestRelativeMotion();
            var ch1 = TestFactory.CreateChannel(Color.White, 800, 400);
            throw new NotImplementedException();

            /*
            //ch1.PostEffects.Add( TTGame.Instance.Content.Load<Effect>("FixedColor") );
            t1.Create();

            // main channel: shows the child channels as sprites
            BuildToDefault();
            var scr1 = TestFactory.CreateSpritelet(ch1);
            scr1.GetComponent<PositionComp>().Position = new Vector2(50f, 50f);
            //scr1.GetComponent<VelocityComp>().Velocity2D = new Vector2(5f, 0.5f);
            */

        }

    }
}
