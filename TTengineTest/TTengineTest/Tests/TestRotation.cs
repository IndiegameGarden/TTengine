using System;
using Microsoft.Xna.Framework;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestRotation : Test
    {
        const float MOVE_SPEED_MULTIPLIER = 80f;

        public override void Create()
        {
            Factory.BallSprite = "paul-hardman_circle-four";

            for (float x = 250f; x < 800f; x += 200f)
            {
                for (float y = 150f; y < 668f; y += 200f)
                {
                    var velo = Vector2.Zero;
                    velo *= MOVE_SPEED_MULTIPLIER;
                    var ball = Factory.CreateMovingBall(new Vector2(x, y), velo );
                    ball.GetComponent<ScaleComp>().Scale = 0.15;
                    ball.AddComponent(new RotateComp());

                    // modifier to adapt rotation
                    TestFactory.AddScript(ball, MyRotateBallScript);
                }
            }
        }

        void MyRotateBallScript(ScriptContext ctx)
        {
            ctx.Entity.GetComponent<RotateComp>().Rotate = ctx.SimTime;
        }

    }
}
