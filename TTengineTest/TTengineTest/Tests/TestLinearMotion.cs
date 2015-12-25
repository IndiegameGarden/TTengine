using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestLinearMotion : Test
    {
        const float MOVE_SPEED_MULTIPLIER = 80f;

        public override void Create()
        {
            Factory.BallSprite = "red-circle_frank-tschakert";
            for (float x = 250f; x < 800f; x += 200f)
            {
                for (float y = 150f; y < 668f; y += 200f)
                {
                    var velo = new Vector2(-0.5f + x / 1024f, -0.5f + y / 768f);
                    velo *= MOVE_SPEED_MULTIPLIER;
                    Factory.CreateMovingBall(new Vector2(x, y), velo );
                }
            }
        }

    }
}
