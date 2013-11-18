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
    /// System that clears Screenlets and opens its spritebatches to begin the draw cycle.
    /// Called first in the Draw() cycle. The ScreenletSystem is executed later.
    /// <seealso cref="ScreenletSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScreenletPreSystem)]
    public class ScreenletPreSystem : EntityComponentProcessingSystem<ScreenComp, DrawComp>
    {

        private GraphicsDevice _gfxDevice;

        protected override void Begin()
        {
            _gfxDevice = TTGame.Instance.GraphicsDevice;
        }

        public override void Process(Entity screenlet, ScreenComp screenComp, DrawComp drawComp)
        {
            // check if present screenComp is the active one in this Draw() round
            if (!screenlet.IsActive)
                return;

            _gfxDevice.SetRenderTarget(screenComp.RenderTarget);
            _gfxDevice.Clear(screenComp.BackgroundColor);

            // in this initial round, start the drawing to this screenlet's spritebatch:
            TTSpriteBatch sb = screenComp.SpriteBatch;
            sb.BeginParameterized();

        }

    }

    /// <summary>
    /// System that handles rendering on Screenlets and from Screenlet to the main display.
    /// Called last in the Draw() cycle. The ScreenletPreSystem is executed before any
    /// drawing on the screens take place.
    /// <seealso cref="ScreenletPreSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScreenletSystem)]
    public class ScreenletSystem : EntityComponentProcessingSystem<ScreenComp, DrawComp>
    {

        public override void Process(Entity entity, ScreenComp screen, DrawComp drawComp)
        {
            // check if present screenComp is the active one in this Draw() round
            if (!screen.IsActive)
                return;
            
            // in this final round, end the drawing to this screenlet:
            TTSpriteBatch sb = screen.SpriteBatch;
            sb.End();

            // then render the screenbuffer onto the actual screen.
            TTGame.Instance.GraphicsDevice.SetRenderTarget(null);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            sb.Draw(screen.RenderTarget, screen.ScreenRectangle, drawComp.DrawColor);
            sb.End();

        }

    }
}
