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
            var s = new SineFunction();
            s.Frequency = 0.5;
            s.Amplitude = 0.25;
            s.Offset = 1;
            TTFactory.AddModifier(ball, MyScaleModifierScript, s);

            // modifier script to adapt rotation
            TTFactory.AddModifier(ball, MyRotateModifierScript);

            // ball 2
            var ball2 = Factory.CreateMovingBall(new Vector2(695f, 450f), velo);
            ball2.GetComponent<ScaleComp>().Scale = 0.5;

            // script with anonymous delegate code block - for rotation
            TTFactory.AddScript(ball2, delegate(ScriptContext ctx) { 
                    ctx.Entity.GetComponent<DrawComp>().DrawRotation = (float)ctx.SimTime; 
                });

            // TargetModifier to set its position towards a target
            var tm = new TargetModifier<PositionComp>(delegate(PositionComp pc, Vector3 pos) { pc.Position = pos; }, 
                                ball2.GetComponent<PositionComp>());
            tm.Target = new Vector3(0f, 0f, 0.2f);
            tm.Value = ball2.GetComponent<PositionComp>().Position;
            tm.Speed = 40;
            tm.AttachTo(ball2);

        }

        void MyScaleModifierScript(ScriptContext ctx)
        {
            ctx.Entity.GetComponent<ScaleComp>().ScaleModifier *= (0.4 + ctx.FunctionValue * 0.3);            
        }

        void MyRotateModifierScript(ScriptContext ctx)
        {
            ctx.Entity.GetComponent<DrawComp>().DrawRotation = (float)ctx.FunctionValue;
        }

    }
}
