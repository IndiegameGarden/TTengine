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
    /// Called first in the Draw() cycle.
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
            // in this initial round, start the drawing to this screenlet's spritebatch:
            TTSpriteBatch sb = screenComp.SpriteBatch;
            sb.BeginParameterized();
        }

    }

    /// <summary>
    /// Called after all Draw() calls to close open spritebatches.
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
            // in this middle round, end the drawing to screenlets with RenderTargets
            //if (screenComp.RenderTarget != null)
            //{
                TTSpriteBatch sb = screenComp.SpriteBatch;
                _gfxDevice.SetRenderTarget(screenComp.RenderTarget);
                //_gfxDevice.Clear(screenComp.BackgroundColor);
                sb.End(); // for deferred spritebatches, this draws everything now to the just-set RenderTarget
                _gfxDevice.SetRenderTarget(null);
            //}
        }

    }


    /// <summary>
    /// ScreenletPostSystem is to render all Screenlet buffers to the main display/backbuffer.
    /// <seealso cref="ScreenletPreSystem"/>
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScreenletPostSystem)]
    public class ScreenletPostSystem : EntityComponentProcessingSystem<ScreenComp, DrawComp>
    {
        private GraphicsDevice _gfxDevice;
        private ScreenComp _defaultDrawScreen;

        protected override void Begin()
        {
            _gfxDevice = TTGame.Instance.GraphicsDevice;
            _defaultDrawScreen = TTGame.Instance.DrawScreen;
        }

        public override void Process(Entity entity, ScreenComp screenComp, DrawComp drawComp)
        {
            if (screenComp.RenderTarget != null)
            {
                if (screenComp.Visible)
                {
                    // in case a RenderTarget is defined: render the screenbuffer onto the actual screen
                    TTSpriteBatch sb = screenComp.SpriteBatch; // _defaultDrawScreen.SpriteBatch;
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    sb.Draw(screenComp.RenderTarget, drawComp.DrawPosition, drawComp.DrawColor);
                    _gfxDevice.SetRenderTarget(null);
                    sb.End();
                }
            }
            else
            {
                // end spritebatch of 'regular' Screenlets that draw to BackBuffer
                //TTSpriteBatch sb = screenComp.SpriteBatch;
                //sb.End();
            }
        }

    }

}
