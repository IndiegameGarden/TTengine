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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.AnimatedSpriteSystem)]
    public class AnimatedSpriteSystem : EntityComponentProcessingSystem<AnimatedSpriteComp, PositionComp, DrawComp>
    {

        protected ScreenComp activeScreen = null;

        protected override void Begin()
        {
            activeScreen = TTGame.Instance.DrawScreen;
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity, AnimatedSpriteComp spriteComp, PositionComp posComp, DrawComp drawComp)
        {
            if (!drawComp.IsVisible)
                return;

            ScreenComp screen = drawComp.DrawScreen;

            // if no specific screen...
            if (screen == null)
                screen = activeScreen;
            // update drawpos
            var p = posComp.PositionAbs;
            drawComp.DrawPosition = screen.ToPixels(p);
            drawComp.LayerDepth = p.Z; // Z position is translated to a layer depth

            spriteComp.frameSkipCounter--;
            if (spriteComp.frameSkipCounter == 0)
            {
                spriteComp.frameSkipCounter = spriteComp.SlowdownFactor;

                // update frame counter - one per frame
                switch (spriteComp.AnimType)
                {
                    case AnimationType.NORMAL:
                        spriteComp.CurrentFrame++;
                        if (spriteComp.CurrentFrame > spriteComp.MaxFrame || spriteComp.CurrentFrame == spriteComp.TotalFrames)
                            spriteComp.CurrentFrame = spriteComp.MinFrame;
                        break;

                    case AnimationType.REVERSE:
                        spriteComp.CurrentFrame--;
                        if (spriteComp.CurrentFrame < spriteComp.MinFrame || spriteComp.CurrentFrame < 0)
                            spriteComp.CurrentFrame = spriteComp.MaxFrame;
                        break;

                    case AnimationType.PINGPONG:
                        spriteComp.CurrentFrame += spriteComp.pingpongDelta;
                        if (spriteComp.CurrentFrame > spriteComp.MaxFrame || spriteComp.CurrentFrame == spriteComp.TotalFrames)
                        {
                            spriteComp.CurrentFrame -= 2;
                            spriteComp.pingpongDelta = -spriteComp.pingpongDelta;
                        }
                        else if (spriteComp.CurrentFrame < spriteComp.MinFrame || spriteComp.CurrentFrame < 0)
                        {
                            spriteComp.CurrentFrame += 2;
                            spriteComp.pingpongDelta = -spriteComp.pingpongDelta;
                        }
                        break;
                }
            }

            // draw sprite from sprite atlas
            TTSpriteBatch sb = screen.SpriteBatch;
            int row = (int)((float)spriteComp.CurrentFrame / (float)spriteComp.Nx);
            int column = spriteComp.CurrentFrame % spriteComp.Nx;
            Vector2 dp = drawComp.DrawPosition;
            Rectangle sourceRectangle = new Rectangle(spriteComp.px * column, spriteComp.py * row, spriteComp.px, spriteComp.py);

            sb.Draw(spriteComp.Texture, dp, sourceRectangle, drawComp.DrawColor,
                drawComp.DrawRotation, spriteComp.drawCenter, drawComp.DrawScale, SpriteEffects.None, drawComp.LayerDepth);

        }

    }
}