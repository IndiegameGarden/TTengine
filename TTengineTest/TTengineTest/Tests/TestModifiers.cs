using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;
using Artemis;

namespace TTengineTest
{
    /// <summary></summary>
    class TestModifiers : Test
    {

        public TestModifiers()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            Factory.BallSprite = "paul-hardman_circle-four";

            // ball 1
            var velo = new Vector2(3f, 0.3f);
            var ball = Factory.CreateMovingBall(new Vector2(95f, 250f), velo);

            // Modifier: adapting scale with sine rhythm
            var s = new SineModifier<ScaleComp>(MyScaleModifier2, ball.GetComponent<ScaleComp>());
            s.Frequency = 0.5;
            s.Amplitude = 0.25;
            s.Offset = 1;
            s.AttachTo(ball);

            // modifier to adapt rotation
            var r = new Modifier<DrawComp>(MyRotateModifier, ball.GetComponent<DrawComp>());
            r.AttachTo(ball);

            // ball 2
            var ball2 = Factory.CreateMovingBall(new Vector2(695f, 450f), velo);
            ball2.GetComponent<ScaleComp>().Scale = 0.5;

            // modifier with anonymous delegate code block 
            var m = new Modifier<DrawComp>(delegate(DrawComp c, double val) { c.DrawRotation = (float)val; },
                                            ball2.GetComponent<DrawComp>());
            m.AttachTo(ball2);

        }

        public void MyScaleModifier(Entity entity, double value)
        {
            entity.GetComponent<ScaleComp>().ScaleModifier *= 0.5 + entity.GetComponent<PositionComp>().Position.X;
        }

        public void MyScaleModifier2(ScaleComp sc, double value)
        {
            sc.ScaleModifier *= (0.4 + value * 0.3);
        }

        public void MyRotateModifier(DrawComp drawComp, double value)
        {
            drawComp.DrawRotation = (float)value;
        }

    }
}
