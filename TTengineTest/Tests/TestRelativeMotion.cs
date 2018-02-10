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

        public override void BuildAll()
        {
            BallSprite = "red-circle";

            // parent ball
            var velo = new Vector2(3f,0.3f);
            var ball = CreateMovingBall(New(), new Vector2(35f, 250f), velo );
            ball.C<ScaleComp>().Scale = 0.15f;

            // child ball 1
            var cball = CreateMovingBall(New(), new Vector2(200f, 0f), Vector2.Zero);
            cball.C<ScaleComp>().Scale = 0.1f;
            cball.C<PositionComp>().Depth = 0f;
            AddScript(cball,CirclingPositionScript);

            // set parent-child relation for the position
            ball.C<PositionComp>().AddChild(cball.C<PositionComp>());

            // child ball 2
            var cball2 = CreateMovingBall(New(), new Vector2(200f, 0f), Vector2.Zero);
            cball2.C<ScaleComp>().Scale = 0.07f;
            cball2.C<PositionComp>().Depth = 0f;
            AddScript(cball2, CirclingPositionScript2);

            // set parent-child relation for the position
            cball.C<PositionComp>().AddChild(cball2.C<PositionComp>());

        }

        void CirclingPositionScript(ScriptComp ctx)
        {
            const float R = 50f;
            const double F = 0.1;
            double t = ctx.SimTime ;
            ctx.Entity.C<PositionComp>().PositionXY = 
                new Vector2((float)(R * Math.Sin(MathHelper.TwoPi * F * t)) , 
                            (float)(R * Math.Cos(MathHelper.TwoPi * F * t)));
        }

        void CirclingPositionScript2(ScriptComp ctx)
        {
            const float R = 30f;
            const double F = 0.14;
            double t = ctx.SimTime;
            ctx.Entity.C<PositionComp>().PositionXY = 
                new Vector2((float)(R * Math.Sin(MathHelper.TwoPi * F * t)), 
                            (float)(R * Math.Cos(MathHelper.TwoPi * F * t)));
        }

    }
}
