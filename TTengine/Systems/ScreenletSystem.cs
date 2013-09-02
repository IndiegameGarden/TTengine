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
    /// <summary>
    /// System that handles rendering on Screenlets and from Screenlet to the main display.
    /// Called last in the Draw() cycle. The ScreenletPreSystem is executed before any
    /// drawing on the screens take place.
    /// <seealso cref="ScreenletPreSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 2)]
    public class ScreenletSystem : EntityComponentProcessingSystem<ScreenComp, DrawComp>
    {

        public override void Process(Entity entity, ScreenComp screen, DrawComp drawComp)
        {
            // check if present screenComp is the active one in this Draw() round
            if (!screen.IsActive)
                return;
            if (TTGame.Instance.ActiveScreen != entity)
                return;
            
            // in this final round, end the drawing to this screenlet:
            TTSpriteBatch sb = screen.SpriteBatch;
            sb.End();

            // then render the screenbuffer onto the actual screenComp.
            TTGame.Instance.GraphicsDevice.SetRenderTarget(null);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            sb.Draw(screen.RenderTarget, screen.ScreenRectangle, drawComp.DrawColor);
            sb.End();

        }

    }
}
