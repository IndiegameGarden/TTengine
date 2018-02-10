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
    /// <summary>Tests a transparent channel and two transparent sprites right of it. The background should show through.
    /// </summary>
    class TestTransparentChannels : Test
    {
        public override void BuildAll()
        {
            // put some content in the background channel
            var chbg = CreateChannel(New(), this.BackgroundColor);
            using (BuildTo(chbg))
            {
                var t0 = new TestSphereCollision();
                t0.BuildLike(this);
                t0.BuildAll();
            }

            // create a first child channel - should become transparent
            var ch1 = CreateChannel(New(), Color.Transparent, 600, 400);
			ch1.C<PositionComp>().PositionXY = new Vector2(50f, 50f);
            Color c1 = ch1.C<SpriteComp>().GetPixel(0, 0);  // for debug inspection only
            AddScript(ch1, ScriptInspectTextureColor);
            ch1.C<WorldComp>().Screen.Zoom = 0.5f;

            // second item - a regular sprite with transparency RUNTIME LOADED
            var spr1 = CreateSprite(New(), "red-circle_frank-tschakert_runtime-load.png"); // ("Op-art-circle_Marco-Braun");
            spr1.C<PositionComp>().PositionXY = new Vector2(800f, 50f);
            Color c2 = spr1.C<SpriteComp>().GetPixel(0, 0);  // for debug inspection only

            // 3rd item - a regular compiled content sprite with transparency content pipeline loaded
            var spr2 = CreateSprite(New(), "red-circle_frank-tschakert"); 
            spr2.C<PositionComp>().PositionXY = new Vector2(1050f, 50f);

            // for channel ch1, build some content into it.
            using (BuildTo(ch1))
            {
                var t1 = new TestScaling();
                t1.BuildLike(this);
                t1.BuildAll();
            }
        }

        void ScriptInspectTextureColor(ScriptComp ctx)
        {
            var e = ctx.Entity;
            var sc = e.C<SpriteComp>();
            Color c1 = sc.GetPixel(0, 0);
            if (c1.A != 0)
            {
                throw new Exception("Expected Alpha=0 in texture pixel (0,0)");
            }
        }


    }
}
