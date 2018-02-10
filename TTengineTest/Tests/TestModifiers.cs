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

        public override void BuildAll()
        {
            BallSprite = "paul-hardman_circle-four";

            // ball 1
            var velo = new Vector2(3f, 0.3f);
            var b1 = CreateMovingBall(New(), new Vector2(95f, 250f), velo);

            // Modifier: adapting scale with sine rhythm
            var sineFunc = new SineFunction { Frequency = 0.5, Amplitude = 0.25, Offset = 1 };
            AddFunctionScript(b1, MyScaleModifierScript, sineFunc);

            // modifier script to adapt rotation
            AddFunctionScript(b1, MyRotateModifierScript, new UnityFunction());

            // ball 2
            var b2 = CreateMovingBall(New(), new Vector2(695f, 450f), velo);
            b2.C<ScaleComp>().Scale = 0.5;

            // script with anonymous delegate code block - for rotation
            AddScript(b2, ctx => { b2.C<DrawComp>().DrawRotation = (float)ctx.SimTime; });

            // TargetModifier to set its position towards a target
            var targFunc = new MoveToTargetFunction();
            targFunc.Target = Vector3.Zero;
            targFunc.CurrentValue = b2.C<PositionComp>().Position;
            targFunc.Speed = 40;
            AddScript(b2, (sc) => { b2.C<PositionComp>().Position = targFunc.Value(sc); } );

            // ball 3
            var b3 = CreateMovingBall(New(), new Vector2(895f, 250f), new Vector2(-1f, 0.3f));
            var f = new SineFunction();
            AddScript(b3, (sc) => { b3.C<ScaleComp>().Scale = 0.43 * f.Value(sc) + 2.0; } );

        }

        void MyScaleModifierScript(ScriptComp ctx, double value)
        {
            ctx.Entity.C<ScaleComp>().Scale = (0.4 + value * 0.3);            
        }

        void MyRotateModifierScript(ScriptComp ctx, double value)
        {
            ctx.Entity.C<DrawComp>().DrawRotation = (float)value;
        }

    }
}
