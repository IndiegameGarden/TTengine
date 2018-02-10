#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpriteRenderSystem.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   The render system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace TTengine.Systems
{
    #region Using statements

    using System;

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using TTengine.Core;
    using TTengine.Comps;

    #endregion

    /// <summary>The system for rendering sprites</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.SpriteRenderSystemDraw)]
    public class SpriteRenderSystem : EntityComponentProcessingSystem<SpriteComp, PositionComp, DrawComp>
    {
        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity, SpriteComp spriteComp, PositionComp posComp, DrawComp drawComp)
        {
            if (!drawComp.IsVisible)
                return;

            var scr = drawComp.DrawScreen;
            
            // update drawpos interpolated - FIXME dont do this repeatedly in systems
            var p = posComp.PositionAbs;
            float tlag = (float)TTGame.Instance.TimeLag;
            if (tlag > 0f && entity.HasC<VelocityComp>())
                p += tlag * entity.C<VelocityComp>().Velocity;
            drawComp.DrawPosition = p; 

            TTSpriteBatch sb = scr.SpriteBatch;

            // draw sprite
            sb.Draw(spriteComp.Texture, drawComp.DrawPositionXY, null, drawComp.DrawColor,
                drawComp.DrawRotation, spriteComp.Center, drawComp.DrawScale, SpriteEffects.None, drawComp.LayerDepth);

        }

    }

    /// <summary>The system for rendering rect sprites</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.SpriteRenderSystemDraw)]
    public class SpriteRectRenderSystem : EntityComponentProcessingSystem<SpriteRectComp, PositionComp, DrawComp>
    {
        protected Texture2D dummyTexture = new Texture2D(TTGame.Instance.GraphicsDevice, 1, 1);
        protected Rectangle rect = new Rectangle();

        public override void LoadContent()
        {
            Color[] data = new Color[] { Color.White };
            dummyTexture.SetData<Color>(data);
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity, SpriteRectComp sprComp, PositionComp pc, DrawComp dc)
        {
            if (!dc.IsVisible)
                return;

            var scr = dc.DrawScreen;

            // update drawpos interpolated FIXME do only once instead of in multiple systems.
            var p = pc.PositionAbs;
            float tlag = (float)TTGame.Instance.TimeLag;
            if (tlag > 0f && entity.HasC<VelocityComp>())
                p += tlag * entity.C<VelocityComp>().Velocity;
            dc.DrawPosition = p;
            dc.LayerDepth = pc.Depth;

            TTSpriteBatch sb = scr.SpriteBatch;

            // draw rect sprite
            rect.X = (int)Math.Round(dc.DrawPosition.X);
            rect.Y = (int)Math.Round(dc.DrawPosition.Y);
            if (sprComp.Width == 0 || sprComp.Height == 0)
            {
                rect.Width = scr.Width;
                rect.Height = scr.Height;
            } else { 
                rect.Width = (int)Math.Round(sprComp.Width * dc.DrawScale);
                rect.Height = (int)Math.Round(sprComp.Height * dc.DrawScale);
            }
            sb.Draw(dummyTexture, rect, null, dc.DrawColor,
                dc.DrawRotation, Vector2.Zero, SpriteEffects.None, dc.LayerDepth);
        }

    }

}