using System;
using System.Collections.Generic;

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
    /// System that opens Screenlets' spritebatches to begin the draw cycle.
    /// Called first in the Draw() cycle.
    /// <seealso cref="ScreenletSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScreenletPreSystem)]
    public class ScreenletPreSystem : EntityComponentProcessingSystem<ScreenComp>
    {

        public override void Process(Entity screenlet, ScreenComp screenComp)
        {
            TTGame.Instance.GraphicsDevice.SetRenderTarget(screenComp.RenderTarget);
            if (screenComp.RenderTarget!=null) TTGame.Instance.GraphicsDevice.Clear(screenComp.BackgroundColor);

            // in this initial round, start the drawing to this screenlet's spritebatch:
            TTSpriteBatch sb = screenComp.SpriteBatch;
            sb.BeginParameterized();
        }

    }

    /// <summary>
    /// Called after all Draw() calls, to close any open spritebatches.
    /// <seealso cref="ScreenletPreSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScreenletSystem)]
    public class ScreenletSystem : EntityComponentProcessingSystem<ScreenComp>
    {

        public override void Process(Entity entity, ScreenComp screenComp)
        {
            TTGame.Instance.GraphicsDevice.SetRenderTarget(screenComp.RenderTarget);
            if (screenComp.RenderTarget != null) TTGame.Instance.GraphicsDevice.Clear(screenComp.BackgroundColor);
            TTSpriteBatch sb = screenComp.SpriteBatch;
            sb.End(); 
        }

    }
     
}
