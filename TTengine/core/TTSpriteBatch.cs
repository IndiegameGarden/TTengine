﻿using System;
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
        public Effect           effect = null;

        /// <summary>
        /// construct a TTSpriteBatch with default rendering parameters for the batch
        /// </summary>
        public TTSpriteBatch(GraphicsDevice gfx):
            base(gfx)
        {
            // uses default params
        }

        /// <summary>
        /// construct a TTSpriteBatch with custom rendering parameters for the batch
        /// </summary>
        public TTSpriteBatch(GraphicsDevice gfx, BlendState bs , SpriteSortMode ssm = SpriteSortMode.BackToFront, 
            SamplerState ss = null, DepthStencilState dss = null, RasterizerState rs = null, Effect fx = null) : 
            base(gfx)
        {
            spriteSortMode = ssm;
            blendState = bs;
            samplerState = ss;
            depthStencilState = dss;
            rasterizerState = rs;
            effect = fx;
        }

        /// <summary>
        /// Call Begin() on the underlying SpriteBatch with the parameters of this TTSpriteBatch applied in the Begin() call
        /// </summary>
        public void BeginParameterized()
        {
            Begin(spriteSortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
        }
        
    }

}
