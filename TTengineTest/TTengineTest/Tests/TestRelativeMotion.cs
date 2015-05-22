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
            Factory.BallSprite = "red-circle";

            // parent ball
            var velo = new Vector2(3f,0.3f);
            var ball = Factory.CreateMovingBall(new Vector2(35f, 250f), velo );
            ball.GetComponent<ScaleComp>().Scale = 0.15f;

            // child ball 1
            var cball = Factory.CreateMovingBall(new Vector2(200f, 0f), Vector2.Zero);
            cball.GetComponent<ScaleComp>().Scale = 0.1f;
            cball.GetComponent<PositionComp>().Depth = 0f;
            TTFactory.AddScript(cball,CirclingPositionScript);

            // set parent-child relation for the position
            ball.GetComponent<PositionComp>().AddChild(cball.GetComponent<PositionComp>());

            // child ball 2
            var cball2 = Factory.CreateMovingBall(new Vector2(200f, 0f), Vector2.Zero);
            cball2.GetComponent<ScaleComp>().Scale = 0.07f;
            cball2.GetComponent<PositionComp>().Depth = 0f;
            TTFactory.AddScript(cball2, CirclingPositionScript2);

            // set parent-child relation for the position
            cball.GetComponent<PositionComp>().AddChild(cball2.GetComponent<PositionComp>());

        }

        void CirclingPositionScript(ScriptContext ctx)
        {
            const float R = 50f;
            const double F = 0.1;
            double t = ctx.SimTime ;
            ctx.Entity.GetComponent<PositionComp>().Position = 
                new Vector2((float)(R * Math.Sin(MathHelper.TwoPi * F * t)) , 
                            (float)(R * Math.Cos(MathHelper.TwoPi * F * t)));
        }

        void CirclingPositionScript2(ScriptContext ctx)
        {
            const float R = 30f;
            const double F = 0.14;
            double t = ctx.SimTime;
            ctx.Entity.GetComponent<PositionComp>().Position = 
                new Vector2((float)(R * Math.Sin(MathHelper.TwoPi * F * t)), 
                            (float)(R * Math.Cos(MathHelper.TwoPi * F * t)));
        }

    }
}
