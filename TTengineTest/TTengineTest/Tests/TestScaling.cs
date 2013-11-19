using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestScaling : Test
    {
        const float MOVE_SPEED_MULTIPLIER = 80f;

        public TestScaling()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            Factory.BallSprite = "red-circle_frank-tschakert";
            for (float x = 250f; x < 800f; x += 200f)
            {
                for (float y = 150f; y < 668f; y += 200f)
                {
                    var b = Factory.CreateMovingBall(new Vector2(x, y), Vector2.Zero);
                    b.AddComponent(new ScaleComp());

                    // to each ball, we add a sinusoid based modification of the Scale parameter.
                    // FIXME try a ref to sc.Scale passing. Simpler.
                    var m = new SineModifier<ScaleComp>(delegate (ScaleComp sc, double value)  { sc.Scale = value; }, 
                                                        b.GetComponent<ScaleComp>());
                    m.Amplitude = RandomMath.RandomBetween(0.1f, 0.8f);
                    m.Frequency = RandomMath.RandomBetween(0.2f, 1.5f);
                    m.Phase = RandomMath.RandomBetween(0f, MathHelper.TwoPi);
                    m.AttachTo(b); // attach modifier to the ball.
                }
            }

        }

    }
}
