﻿using System;
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

        public override void BuildAll()
        {
            BallSprite = "paul-hardman_circle-four";
            double spd = 0.2;
            for (float x = 250f; x < 800f; x += 200f)
            {
                for (float y = 150f; y < 668f; y += 200f)
                {
                    var velo = Vector2.Zero;
                    velo *= MOVE_SPEED_MULTIPLIER;
                    var ball = CreateMovingBall(New(), new Vector2(x, y), velo );
                    ball.C<ScaleComp>().Scale = 0.19;
                    var rc = new RotateComp();
                    rc.RotateSpeed = spd;
                    ball.AddC(rc);
                    spd *= 1.678;
                }
            }
        }

    }
}
