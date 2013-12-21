using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Zooms in on a part of a rendered Channel. Useful for e.g. scrolling level.</summary>
    class TestZoomedChannel : Test
    {

        public TestZoomedChannel()
            : base()        
        {
            BackgroundColor = Color.DarkGray;
        }

        public override void Create()
        {
            var ch = TTFactory.BuildChannel;
            ch.Screen.SpriteBatch.samplerState = SamplerState.PointClamp;

            // child channel
            //var t1 = new TestRotation();
            var ch1 = TTFactory.CreateChannel(640, 480, Color.Black);
            TTFactory.BuildTo(ch1);
            var s = TTFactory.CreateSpritelet("Quest14-Level1.png");
            ch.AddChild(ch1);

            // main channel: shows the child channel sprite
            TTFactory.BuildTo(ch);
            var scr1 = TTFactory.CreateSpritelet(ch1);
            // select a place in the level bitmap.
            scr1.GetComponent<SpriteComp>().Center = new Vector2(532f, 227f);
            scr1.AddComponent(new ScaleComp(1.0));
            scr1.GetComponent<PositionComp>().Position = ch.Screen.Center;
            scr1.GetComponent<PositionComp>().Z = 1f; // background
            scr1.GetComponent<ScaleComp>().ScaleTarget = 14.0;
            scr1.GetComponent<ScaleComp>().ScaleSpeed = 0.01;
            scr1.Refresh();

            var t1 = new TestRotation();
            t1.Create();
        }

    }
}
