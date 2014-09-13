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
    /// <summary></summary>
    class TestGamepad : Test
    {
        IntPtr joy0;

        public TestGamepad()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var b = Factory.CreateMovingBall(TTFactory.BuildScreen.Center, Vector2.Zero);
            TTFactory.AddScript(b, ScriptMoveByGamepad);

            /*
            int retval = Sdl.SDL_Init(Sdl.SDL_INIT_JOYSTICK | Sdl.SDL_INIT_VIDEO );
            if (retval != 0)
            {
                TTGame.Instance.Exit();
            }
            //Sdl.SDL_JoystickEventState(Sdl.SDL_ENABLE);
            //Sdl.SDL_JoystickEventState(Sdl.SDL_DISABLE);
            int num = Tao.Sdl.Sdl.SDL_NumJoysticks();
            joy0 = Sdl.SDL_JoystickOpen(0);
            if (num == 0)
            {
                TTGame.Instance.Exit();
            }
             */

        }

        void ScriptMoveByGamepad(ScriptContext ctx)
        {
            var e = ctx.Entity;
            var vc = e.GetComponent<VelocityComp>();
            float spd = (float)(95 * ctx.Dt);

            /*
            byte x = Tao.Sdl.Sdl.SDL_JoystickGetButton(joy0, 255);
            if (x > 0)
            {
                vc.Y -= spd;
            }
             */

            /*
            Tao.Sdl.Sdl.SDL_Event ev = new Tao.Sdl.Sdl.SDL_Event();
            Sdl.SDL_PollEvent(out ev);
            if (ev.type == Sdl.SDL_JOYBUTTONDOWN)
            {
                vc.Y -= spd;
            }
            else if (ev.type != 0)
            {
                Sdl.SDL_PushEvent(out ev); // put back
            }
            */

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
