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

            // dedicated channel for rendering the level using blocky (non interpolated) graphics bitmap
            var ch1 = TTFactory.CreateChannel(Color.Black,true);
            ch.AddChild(ch1);
            ch1.Screen.SpriteBatch.samplerState = SamplerState.PointClamp; // nice 'n blocky
            TTFactory.BuildTo(ch1);
            var s = TTFactory.CreateSpritelet("Quest14-Level1.png");
            s.GetComponent<SpriteComp>().Center = new Vector2(532f, 227f);
            s.AddComponent(new ScaleComp(1.0));
            //var mod = new TargetModifier<ScaleComp>(delegate(ScaleComp sc, Vector3 val) { sc.Scale = val.X; }, s.GetComponent<ScaleComp>());
            var targFunc = new MoveToTargetFunction();
            targFunc.CurrentValue.X = 1.0f;
            targFunc.Target.X = 15.0f;
            targFunc.Speed = 3;
            TTFactory.AddModifier(s, delegate(ScriptContext ctx, Vector2 val) { ctx.Entity.GetComponent<ScaleComp>().Scale = val.X; },
                targFunc);
            s.GetComponent<PositionComp>().Position = ch.Screen.Center;

            // -- main channel: shows the child channel using a sprite
            TTFactory.BuildTo(ch);
            var scr1 = TTFactory.CreateSpritelet(ch1);
            scr1.GetComponent<PositionComp>().Depth = 0.9f;
            // some non-blocky graphics in front of level; using default Spritebatch
            var t1 = new TestAnimatedSprite();
            t1.Create();                    

        }

    }
}
