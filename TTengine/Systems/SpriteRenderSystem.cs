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

    using TTengine.Comps;

    #endregion

    /// <summary>The system for rendering sprites</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    public class SpriteRenderSystem : EntityComponentProcessingSystem<SpriteComp, PositionComp, DrawComp>
    {

        /// <summary>Override to implement code that gets executed when systems are initialized.</summary>
        public override void LoadContent()
        {
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity, SpriteComp spriteComp, PositionComp posComp, DrawComp drawComp)
        {
            if (drawComp.IsVisible)
            {
                // update drawpos
                drawComp.DrawPosition = drawComp.ToPixels(posComp.Position + posComp.PositionModifier);

                // draw sprite
                drawComp.Screen.SpriteBatch.Draw(spriteComp.Texture, drawComp.DrawPosition, null, drawComp.DrawColor,
                    0f, spriteComp.DrawCenter, drawComp.DrawScale, SpriteEffects.None, drawComp.LayerDepth);
            }
        }

    }
}