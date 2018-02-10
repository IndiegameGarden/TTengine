using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Artemis;
using TTengine.Comps;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestContentLoad : Test
    {
        public override void BuildAll()
        {
            BallSprite = "red-circle_frank-tschakert";
            var b1 = CreateMovingBall(New(), new Vector2(400f, 300f), new Vector2(-8f,0f), 1,0.11f);

            BallSprite = "red-circle_frank-tschakert_runtime-load.png";
            var b2 = CreateMovingBall(New(), new Vector2(600f, 300f), new Vector2(8f,0f),1,0.1f);

            var p = new Vector2(0f, 100f);
            var t = CreateText(New(), p, "XNA Content pipeline", Color.Black,0.11f);
            t.C<ScaleComp>().Scale = 0.5f;
            b1.C<PositionComp>().AddChild(t.C<PositionComp>());

            var t2 = CreateText(New(), p, "Runtime loaded PNG", Color.Black,0.1f);
            t2.C<ScaleComp>().Scale = 0.5f;
            b2.C<PositionComp>().AddChild(t2.C<PositionComp>());
        }

    }
}
