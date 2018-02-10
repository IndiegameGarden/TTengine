// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Zooms in on a part of a rendered Screenlet. Useful for e.g. scrolling level.</summary>
    class TestZoomedScreen : Test
    {

        public TestZoomedScreen()
        {
            BackgroundColor = Color.DarkGray;
        }

        public override void BuildAll()
        {
            // dedicated screen for rendering the level using blocky (non interpolated) graphics bitmap
            var levScr = CreateScreen(New(), Color.Black,true);
            levScr.C<ScreenComp>().SpriteBatch.samplerState = SamplerState.PointClamp; // nice 'n blocky
            using (BuildTo(levScr))
            {
                var s = CreateSprite(New(), "Quest14-Level1.png");
                s.C<SpriteComp>().Center = new Vector2(532f, 227f);
                s.AddC(new ScaleComp(1.0));
                var targFunc = new MoveToTargetFunction
                {
                    CurrentValue = new Vector3(1.0f, 0f, 0f),
                    Target = new Vector3(15.0f, 0f, 0f),
                    Speed = 1
                };
                AddScript(s, (ctx) => { s.C<ScaleComp>().Scale = targFunc.Value(ctx).X; });
                s.C<PositionComp>().PositionXY = Channel.C<WorldComp>().Screen.Center;
            }

            // -- main channel: shows the child channel using a sprite
            var scr1 = CreateSprite(New(), levScr.C<ScreenComp>());
            scr1.C<PositionComp>().Depth = 0.9f;
            // some non-blocky graphics in front of level; using default Spritebatch
            var t1 = new TestAnimatedSprite();
            t1.BuildLike(this);
            t1.BuildAll();                    

        }

    }
}
