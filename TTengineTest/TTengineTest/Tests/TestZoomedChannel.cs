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
            var ch = TTFactory.BuildChannel; // parent channel

            // dedicated channel for rendering the level
            var ch1 = TTFactory.CreateChannel(Color.Black,true); 
            ch1.Screen.SpriteBatch.samplerState = SamplerState.PointClamp; // nice 'n blocky
            ch.AddChild(ch1);
            TTFactory.BuildTo(ch1);
            var s = TTFactory.CreateSpritelet("Quest14-Level1.png");
            s.GetComponent<SpriteComp>().Center = new Vector2(532f, 227f);
            s.AddComponent(new ScaleComp(14.0));
            s.GetComponent<PositionComp>().Position = ch.Screen.Center;
            s.Refresh();

            // -- main channel: shows the child channel using a sprite
            TTFactory.BuildTo(ch);
            // some non-blocky graphics in front of level; using default Spritebatch
            //var t1 = new TestRotation();
            //t1.Create();                    
            var scr1 = TTFactory.CreateSpritelet(ch1);

        }

    }
}
