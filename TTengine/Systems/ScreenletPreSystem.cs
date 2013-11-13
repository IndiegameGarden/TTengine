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
}
