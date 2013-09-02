using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;
using TTengine.Comps;

using Artemis;
using Artemis.Manager;
using Artemis.Attributes;
using Artemis.System;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    public class ScreenletPreSystem : EntityComponentProcessingSystem<ScreenComp, DrawComp>
    {

        public override void Process(Entity entity, ScreenComp screen, DrawComp drawComp)
        {
            // check if present screen is the active one in this Draw() round
            if (!screen.IsActive) 
                return;
            if (TTGame.Instance.ActiveScreen != entity)
                return;

            // in this initial round, start the drawing to this screenlet's spritebatch:
            TTSpriteBatch sb = screen.SpriteBatch;
            sb.BeginParameterized();

        }

    }
}
