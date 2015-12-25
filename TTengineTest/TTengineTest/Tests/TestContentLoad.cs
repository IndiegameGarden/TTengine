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
        public override void Create()
        {
            Factory.BallSprite = "red-circle_frank-tschakert";
            var b1 = Factory.CreateMovingBall(new Vector2(400f, 300f), new Vector2(-8f,0f));

            Factory.BallSprite = "red-circle_frank-tschakert_runtime-load.png";
            var b2 = Factory.CreateMovingBall(new Vector2(600f, 300f), new Vector2(8f,0f));

            var p = new Vector2(0f, 100f);
            var t = Factory.CreateTextlet(p, "XNA Content pipeline", Color.Black);
            t.GetComponent<ScaleComp>().Scale = 0.5f;
            b1.GetComponent<PositionComp>().AddChild(t.GetComponent<PositionComp>());

            var t2 = Factory.CreateTextlet(p, "Runtime loaded PNG", Color.Black);
            t2.GetComponent<ScaleComp>().Scale = 0.5f;
            b2.GetComponent<PositionComp>().AddChild(t2.GetComponent<PositionComp>());
        }

    }
}
