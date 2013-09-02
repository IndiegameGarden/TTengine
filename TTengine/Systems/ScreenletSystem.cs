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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 1)]
    public class ScreenletSystem : EntityComponentProcessingSystem<ScreenletComp, DrawComp>
    {

        public override void Process(Entity entity, ScreenletComp screen, DrawComp drawComp)
        {
            if (!screen.IsActive) return;
            // FIXME code here !?
            TTGame.Instance.GraphicsDevice.SetRenderTarget(null);
            // TODO 
            //render // the buffer to screen
            //spritebatch needed.
            // FIXME what approach?
            //screen.SpriteBatch.BeginParameterized();
            //screen.SpriteBatch.Draw(screen.RenderTarget, screen.ScreenRectangle, drawComp.DrawColor);
            //screen.SpriteBatch.End();

            // FIXME must be at very end, after all sprite draws etc are done
            screen.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            screen.SpriteBatch.Draw(screen.RenderTarget, screen.ScreenRectangle, drawComp.DrawColor); 
            screen.SpriteBatch.End();

        }

    }
}
