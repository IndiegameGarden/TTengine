using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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

            // in this initial round, start the drawing to this screenlet's spritebatch:
            TTSpriteBatch sb = screenComp.SpriteBatch;
            _gfxDevice.SetRenderTarget(screenComp.RenderTarget);
            _gfxDevice.Clear(screenComp.BackgroundColor);
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

        private GraphicsDevice _gfxDevice;

        protected override void Begin()
        {
            _gfxDevice = TTGame.Instance.GraphicsDevice;
        }

        public override void Process(Entity entity, ScreenComp screenComp, DrawComp drawComp)
        {
            // in this final round, end the drawing to this screenlet:
            TTSpriteBatch sb = screenComp.SpriteBatch;
            _gfxDevice.SetRenderTarget(screenComp.RenderTarget);
            _gfxDevice.Clear(screenComp.BackgroundColor);
            sb.End(); // for deferred spritebatches, this draws everything now to the set RenderTarget

            if (screenComp.RenderTarget != null && screenComp.Visible)
            {
                // in case a RenderTarget is defined: render the screenbuffer onto the actual screen
                sb.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
                sb.Draw(screenComp.RenderTarget, drawComp.DrawPosition, drawComp.DrawColor);
                _gfxDevice.SetRenderTarget(null);
                sb.End();
            }
        }

    }
}
