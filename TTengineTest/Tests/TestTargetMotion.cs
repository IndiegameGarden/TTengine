using System;
using Microsoft.Xna.Framework;
using TTengine.Comps;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestTargetMotion : Test
    {
        const float MOVE_SPEED_MULTIPLIER = 80f;

        public override void BuildAll()
        {
            BallSprite = "red-circle_frank-tschakert";
            var velo = new Vector2(-0.2f,-0.05f);
            var pos = new Vector2(300f, 300f);
            velo *= MOVE_SPEED_MULTIPLIER;
            var ball = CreateMovingBall(New(), pos, velo );
            ball.AddC(new TargetMotionComp());
            ball.C<TargetMotionComp>().Target = new Vector3(800f, 500f, 0.5f);
            ball.C<TargetMotionComp>().TargetVelocity = 2*MOVE_SPEED_MULTIPLIER;
        }

    }
}
