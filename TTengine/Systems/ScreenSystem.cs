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
    /// System that opens Screens' spritebatches to begin the draw cycle.
    /// Called first in the Draw() cycle.
    /// <seealso cref="ScreenSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScreenPreSystemDraw)]
    public class ScreenPreSystem : EntityComponentProcessingSystem<ScreenComp>
    {

        public override void Process(Entity e, ScreenComp sc)
        {
            //TTGame.Instance.GraphicsDevice.SetRenderTarget(screenComp.RenderTarget);
            //TTGame.Instance.GraphicsDevice.Clear(screenComp.BackgroundColor);

            // in this initial round, start the drawing to this screen's spritebatch:
            TTSpriteBatch sb = sc.SpriteBatch;
            sb.BeginParameterized();
        }

    }

    /// <summary>
    /// Called after all Draw() calls, to close any open spritebatches.
    /// <seealso cref="ScreenPreSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScreenPostSystemDraw)]
    public class ScreenSystem : EntityComponentProcessingSystem<ScreenComp>
    {

        public override void Process(Entity e, ScreenComp sc)
        {
            TTGame.Instance.GraphicsDevice.SetRenderTarget(sc.RenderTarget);
            TTSpriteBatch sb = sc.SpriteBatch;
            if (sb.effect == null)      // dont clear for FX Screens, since they borrow other's RenderBuffer.
                TTGame.Instance.GraphicsDevice.Clear(sc.BackgroundColor);            
            sb.End(); 
        }

    }
     
}
