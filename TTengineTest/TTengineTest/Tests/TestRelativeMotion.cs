using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestRelativeMotion : Test
    {
        public TestRelativeMotion()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            Factory.BallSprite = "Op-art-circle_Marco-Braun";

            // parent ball
            var velo = new Vector2(3f,0.3f);
            var ball = Factory.CreateMovingBall(new Vector2(55f, 250f), velo );
            ball.GetComponent<ScaleComp>().Scale = 1;

            // child ball
            Factory.BallSprite = "red-circle_frank-tschakert";
            var cball = Factory.CreateMovingBall(new Vector2(100f, 0f), Vector2.Zero);
            cball.GetComponent<ScaleComp>().Scale = 0.5;
            var m = new Modifier<PositionComp>(TrajectoryModifier,cball.GetComponent<PositionComp>());
            m.AttachTo(cball);
        }

        public void TrajectoryModifier(PositionComp rotComp, double value)
        {
            const float R = 50f;
            const double F = 0.1;
            double t = value ;
            rotComp.Position2D = new Vector2((float)(R * Math.Sin(MathHelper.TwoPi * F * t)) , (float)(R * Math.Cos(MathHelper.TwoPi * F * t)));
        }

    }
}
