
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using PXengine.Comps;

namespace PXengine.Systems
{
    /// <summary>'Motion towards a target' system.</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.UserControlSystem)]
    public class UserControlSystem : EntityComponentProcessingSystem<UserControlComp, ControlComp>
    {
        public override void Process(Entity entity, UserControlComp uc, ControlComp cc)
        {
            float dx = 0f, dy = 0f;
            var kb = Keyboard.GetState();
            // FIXME gamepad state also

            // FIXME: ensure most recent pressed new direction key is evaluated here.
            if (kb.IsKeyDown(Keys.Up))
                    dy = -1.0f;
            else if (kb.IsKeyDown(Keys.Down))
                    dy = +1.0f;
            else if (kb.IsKeyDown(Keys.Left))
                    dx = -1.0f;
            else if (kb.IsKeyDown(Keys.Right))
                    dx = +1.0f;

            // trigger Toy
            bool isTriggerKeyPressed = kb.IsKeyDown(Keys.Space) ||
                                    kb.IsKeyDown(Keys.X) ||
                                    kb.IsKeyDown(Keys.LeftControl);

            cc.Move = new Vector2(dx, dy);
            cc.IsSteering = true;

        }
    }

}
