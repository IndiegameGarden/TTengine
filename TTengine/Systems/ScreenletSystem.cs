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
    public class ScreenletSystem : EntityComponentProcessingSystem<ScreenComp, DrawComp>
    {

        public override void Process(Entity entity, ScreenComp screen, DrawComp drawComp)
        {
            if (!screen.IsActive) return;
            // FIXME code here !?
            
            // TODO 
            //render // the buffer to screen
            //spritebatch needed.
            // FIXME what approach?
            //screen.SpriteBatch.BeginParameterized();
            //screen.SpriteBatch.Draw(screen.RenderTarget, screen.ScreenRectangle, drawComp.DrawColor);
            //screen.SpriteBatch.End();

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
