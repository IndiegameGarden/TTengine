using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

//using Tao.Sdl;

namespace TTengineTest
{
    /// <summary>Testing the Gamepad (XInput type only) basics</summary>
    class TestGamepad : Test
    {
        public TestGamepad()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var b = Factory.CreateMovingBall(TTFactory.BuildScreen.Center, Vector2.Zero);
            TTFactory.AddScript(b, ScriptMoveByGamepad);
        }

        void ScriptMoveByGamepad(ScriptContext ctx)
        {
            var e = ctx.Entity;
            var vc = e.GetComponent<VelocityComp>();
            float spd = (float)(195 * ctx.Dt);

            GamePadState gp = GamePad.GetState(PlayerIndex.One);

            if (gp.IsButtonDown(Buttons.DPadUp))
            {
                vc.Y -= spd;
            }
            else if (gp.IsButtonDown(Buttons.DPadDown))
            {
                vc.Y += spd;
            }
            else if (gp.IsButtonDown(Buttons.DPadLeft))
            {
                vc.X -= spd;
            }
            else if (gp.IsButtonDown(Buttons.DPadRight))
            {
                vc.X += spd;
            }

        }
    }
}
