
namespace TTengine.Systems
{
    #region Using statements

    using System;
    using Microsoft.Xna.Framework.Input;
    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;
    using Microsoft.Xna.Framework;
    using TTengine.Comps;

    #endregion

    /// <summary>The movement system.</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.RotateSystem)]
    public class PlayerInputSystem : EntityComponentProcessingSystem<PlayerInputComp>
    {
        double dt = 0;
        KeyboardState   kb = new KeyboardState(), 
                        kbOld;
        GamePadState pad;

        protected override void Begin()
        {
            dt = TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
            kbOld = kb;
            kb = Keyboard.GetState();
            pad = GamePad.GetState(PlayerIndex.One); // TODO assumes single player always
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity, PlayerInputComp pic)
        {
            pic.Direction = Vector2.Zero;
            if (pic.Player != PlayerIndex.One)  // TODO handle input for >1 players
                return;

            // gamepad input
            if (pad.IsConnected)
            {
                if (pad.IsButtonDown(Buttons.DPadLeft) )
                    pic.Direction = -Vector2.UnitX;
                else if (pad.IsButtonDown(Buttons.DPadRight) )
                    pic.Direction = Vector2.UnitX;
                else if (pad.IsButtonDown(Buttons.DPadUp) )
                    pic.Direction = -Vector2.UnitY;
                else if (pad.IsButtonDown(Buttons.DPadDown) )
                    pic.Direction = Vector2.UnitY;
                var stks = pad.ThumbSticks.Left + pad.ThumbSticks.Right;
                stks.Y = -stks.Y;
                pic.Direction += stks;
            }

            // keyboard input
            if (kb.Equals(kbOld))
            {
                if (kb.IsKeyDown(Keys.Left))
                    pic.Direction += -Vector2.UnitX;
                else if (kb.IsKeyDown(Keys.Right))
                    pic.Direction += Vector2.UnitX;
                else if (kb.IsKeyDown(Keys.Up))
                    pic.Direction -= Vector2.UnitY;
                else if (kb.IsKeyDown(Keys.Down))
                    pic.Direction += Vector2.UnitY;
            }
            else
            {
                // key change - adapt to new key smoothly
                if (kb.IsKeyDown(Keys.Left) && !kbOld.IsKeyDown(Keys.Left))
                    pic.Direction += -Vector2.UnitX;
                else if (kb.IsKeyDown(Keys.Right) && !kbOld.IsKeyDown(Keys.Right))
                    pic.Direction += Vector2.UnitX;
                else if (kb.IsKeyDown(Keys.Up) && !kbOld.IsKeyDown(Keys.Up))
                    pic.Direction -= Vector2.UnitY;
                else if (kb.IsKeyDown(Keys.Down) && !kbOld.IsKeyDown(Keys.Down))
                    pic.Direction += Vector2.UnitY;
            }
        }
    }

}