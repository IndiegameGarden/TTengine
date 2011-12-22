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
        protected String effectFile = null;
        protected SpriteBatch spriteBatch;

        protected EffectParameter drawColorParam, timeParam, positionParam;

        /// <summary>
        /// construct with BasicEffect and given texture loaded from content file
        /// </summary>
        /// <param name="textureFile">texture content file</param>
        public EffectSpritelet(String textureFile)
            : base(textureFile)
        {
            this.effectFile = null;
            InitEffect();
        }

        /// <summary>
        /// construct with BasicEffect and given Texture2D
        /// </summary>
        /// <param name="texture">texture content file</param>
        public EffectSpritelet(Texture2D texture)
            : base(texture)
        {
            this.effectFile = null;
            InitEffect();
        }

        /// <summary>
        /// construct with given texture loaded from content file and given shader Effect
        /// </summary>
        /// <param name="textureFile">texture content file</param>
        /// <param name="effectFile">shader effect file</param>
        public EffectSpritelet(String textureFile, String effectFile)
            : base(textureFile)
        {
            this.effectFile = effectFile;
            InitEffect();
        }

        /// <summary>
        /// construct with given Texture2D and shader effect
        /// </summary>
        /// <param name="texture">texture to use</param>
        /// <param name="effectFile">shader effect file to load</param>
        public EffectSpritelet(Texture2D texture, String effectFile)
            : base(texture)
        {
            this.effectFile = effectFile;
            InitEffect();
        }

        /// <summary>
        /// Get reference to the Effect applied
        /// </summary>
        public Effect Eff
        {
            get { return eff; }
        }

        protected virtual void InitEffect()
        {
            if (effectFile != null)
                eff = TTengineMaster.ActiveGame.Content.Load<Effect>(effectFile);
            else
                eff = new BasicEffect(Screen.graphicsDevice);
            VertexShaderInit(eff);

            // try to find my effect-related spritebatch
            if (Screen.effect2spritebatchTable.ContainsKey(eff))
            {
                spriteBatch = Screen.effect2spritebatchTable[eff];
            }
            else
            {
                // create sb and put it in for future use
                spriteBatch = new SpriteBatch(Screen.graphicsDevice);
                Screen.effect2spritebatchTable.Add(eff, spriteBatch);
            }

            // try to find common parameters in the Effect
            drawColorParam = eff.Parameters["DrawColor"];
            timeParam = eff.Parameters["Time"];
            positionParam = eff.Parameters["Position"];
        }

        protected override void OnDraw(ref DrawParams p)
        {
            if (Texture != null)
            {
                // supply the shader parameters that may have been configured
                if (drawColorParam != null)
                    drawColorParam.SetValue(DrawColor.ToVector4());
                if (timeParam != null)
                    timeParam.SetValue(SimTime);
                if (positionParam != null)
                    positionParam.SetValue(Position);
                // retrieve my shared spritebatch for this effect, and draw
                Screen.UseSharedSpritebatch(eff).Draw(Texture, DrawPosition, null, DrawColor,
                       RotateAbs, DrawCenter, DrawScale, SpriteEffects.None, LayerDepth);
            }
        }        

    }
}
