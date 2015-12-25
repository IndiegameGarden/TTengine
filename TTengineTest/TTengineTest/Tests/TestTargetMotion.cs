using System;
using Microsoft.Xna.Framework;
using TTengine.Comps;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestTargetMotion : Test
    {
        const float MOVE_SPEED_MULTIPLIER = 80f;

        public override void Create()
        {
            Factory.BallSprite = "red-circle_frank-tschakert";
            var velo = new Vector2(-0.2f,-0.05f);
            var pos = new Vector2(300f, 300f);
            velo *= MOVE_SPEED_MULTIPLIER;
            var ball = Factory.CreateMovingBall(pos, velo );
            ball.AddComponent(new TargetMotionComp());
            ball.GetComponent<TargetMotionComp>().Target = new Vector2(800f, 500f);
            ball.GetComponent<TargetMotionComp>().TargetVelocity = 2*MOVE_SPEED_MULTIPLIER;
            ball.Refresh();
        }

    }
}
