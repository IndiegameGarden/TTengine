using System;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /**
     * A Spritelet that supports drawing with a given shader effect applied (HLSL)
     */
    public class EffectSpritelet : Spritelet
    {
        protected Effect eff;
        protected String effectFile = "";
        protected SpriteBatch spriteBatch;

        public BlendState blendState = BlendState.AlphaBlend;
        public SpriteSortMode spriteSortMode = SpriteSortMode.BackToFront;

        public EffectSpritelet(String textureFile, String effectFile)
            : base(textureFile)
        {
            this.effectFile = effectFile;
        }

        protected override void OnInit()
        {
            base.OnInit();
            spriteBatch = new SpriteBatch(Screen.graphicsDevice);
            eff = TTengineMaster.ActiveGame.Content.Load<Effect>(effectFile);
            VertexShaderInit(eff);
        }

        protected override void OnDraw(ref DrawParams p)
        {
            spriteBatch.Begin(spriteSortMode, blendState, null, null, null, eff);
            spriteBatch.Draw(Texture, Screen.ToPixels(DrawPosition), null, DrawColor,
                   RotateAbsolute, DrawCenter, DrawScale, SpriteEffects.None, LayerDepth);
            spriteBatch.End();

        }        

    }
}
