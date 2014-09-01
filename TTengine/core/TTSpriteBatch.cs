using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /// <summary>
    /// XNA SpriteBatch including needed parameters to call Begin() on it
    /// </summary>
    public class TTSpriteBatch : SpriteBatch
    {
        public SpriteSortMode   spriteSortMode  = SpriteSortMode.BackToFront;
        public BlendState       blendState      = BlendState.AlphaBlend;
        public SamplerState     samplerState    = null;
        public DepthStencilState depthStencilState = null;
        public RasterizerState  rasterizerState = null;

        public TTSpriteBatch(GraphicsDevice gfx):
            base(gfx)
        {
            // uses default params
        }

        /// <summary>
        /// construct a TTSpriteBatch with user-selected rendering parameters for the batch
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="ssm"></param>
        /// <param name="bs"></param>
        /// <param name="ss"></param>
        /// <param name="dss"></param>
        /// <param name="rs"></param>
        public TTSpriteBatch(GraphicsDevice gfx, SpriteSortMode ssm, BlendState bs, SamplerState ss, DepthStencilState dss, RasterizerState rs): 
            base(gfx)
        {
            spriteSortMode = ssm;
            blendState = bs;
            samplerState = ss;
            depthStencilState = dss;
            rasterizerState = rs;
        }

        /// <summary>
        /// Call Begin() on the underlying SpriteBatch with the parameters of this TTSpriteBatch applied in the Begin() call
        /// </summary>
        public void BeginParameterized()
        {
            Begin(spriteSortMode, blendState, samplerState, depthStencilState, rasterizerState);
        }
        
    }

}
