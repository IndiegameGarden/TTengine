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

        public TestPostEffects()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var ch = TTFactory.BuildChannel;
            TTFactory.BuildTo(ch);

            // create an additional child channel that renders onto the main channel
            // content for 1st screen: call upon another unit test
            var t1 = new TestRelativeMotion();
            var ch1 = TTFactory.CreateChannel(800, 400, Color.White, true);
            ch1.PostEffects.Add( TTGame.Instance.Content.Load<Effect>("RandomColor") );
            TTFactory.BuildTo(ch1);
            t1.Create();
            ch.AddChild(ch1);

            // main channel: shows the child channels as sprites
            TTFactory.BuildTo(ch);
            var scr1 = TTFactory.CreateSpritelet(ch1);
            scr1.GetComponent<PositionComp>().Position = new Vector3(50f, 50f, 0.1f);
            //scr1.GetComponent<VelocityComp>().Velocity2D = new Vector2(5f, 0.5f);


        }

    }
}
