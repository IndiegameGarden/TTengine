using System;
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
                    var m = new SineFunction();
                    m.Amplitude = RandomMath.RandomBetween(0.05f, 0.4f);
                    m.Frequency = RandomMath.RandomBetween(0.04f, 0.3f);
                    m.Phase = RandomMath.RandomBetween(0f, MathHelper.TwoPi);
                    m.Offset = RandomMath.RandomBetween(0.45f, 0.8f);
                    TestFactory.AddModifier(b, delegate(ScriptContext ctx, double value)
                        {ctx.Entity.GetComponent<ScaleComp>().Scale = value;} , m);
                }
            }
        }

    }
}
