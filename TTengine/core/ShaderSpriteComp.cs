using System;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /// <summary>
    /// SpriteComp that supports drawing sprites with a given shader effect applied (HLSL)  
    /// </summary>
    public class ShaderSpriteComp : SpriteComp
    {
        /// <summary>
        /// whether the Effect is used when drawing (true), or not (false).
        /// </summary>
        public bool EffectEnabled = true;

        /// <summary>
        /// the Effect applied (if any)
        /// </summary>
        protected Effect eff = null;

        /// <summary>
        /// filename from which to load .fx Effect (if any)
        /// </summary>
        protected String effectFile = null;

        /// <summary>
        /// Link to certain default parameters in the shader effect
        /// </summary>
        protected EffectParameter timeParam, positionParam;

        internal TTSpriteBatch mySpriteBatch = null;

        /// <summary>
        /// construct with BasicEffect and given texture loaded from content file
        /// </summary>
        /// <param name="textureFile">texture content file</param>
        public ShaderSpriteComp(String textureFile)
            : base(textureFile)
        {
            this.effectFile = null;
            InitEffect();
        }

        /// <summary>
        /// construct with BasicEffect and given Texture2D
        /// </summary>
        /// <param name="texture">texture content file</param>
        public ShaderSpriteComp(Texture2D texture)
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
        public ShaderSpriteComp(String textureFile, String effectFile)
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
        public ShaderSpriteComp(Texture2D texture, String effectFile)
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

        /// <summary>
        /// all Effect initialization goes here
        /// </summary>
        protected virtual void InitEffect()
        {
            if (effectFile != null)
                eff = TTengineMaster.ActiveGame.Content.Load<Effect>(effectFile);
            else
                eff = new BasicEffect(Parent.DrawC.Screen.graphicsDevice);
            Parent.DrawC.VertexShaderInit(eff); // FIXME move away from gamelet

            // find or create my effect-related spritebatch
            MySpriteBatch = Parent.DrawC.Screen.CreateSharedSpriteBatch(eff);

            // try to find common parameters in the Effect (gets null if not found)
            timeParam = eff.Parameters["Time"];
            positionParam = eff.Parameters["Position"];
        }

        protected override void OnDraw(ref DrawParams p)
        {
            if (Texture != null)
            {
                // supply the shader parameters that may have been configured
                // TODO may not be useful with shared spritebatches: whole batch of drawn objects uses latest set shader params only.
                if (timeParam != null)
                    timeParam.SetValue(Parent.SimTime);
                if (positionParam != null)
                    positionParam.SetValue(Parent.Motion.Position);

                MySpriteBatch.Draw(Texture, Parent.DrawC.DrawPosition, null, Parent.DrawC.DrawColor,
                       Parent.Motion.RotateAbs, DrawCenter, Parent.DrawC.DrawScale, SpriteEffects.None, Parent.DrawC.LayerDepth);
            }
        }

        internal TTSpriteBatch MySpriteBatch
        {
            get
            {
                if (EffectEnabled)
                {
                    Parent.DrawC.Screen.UseSharedSpriteBatch(eff);
                    return mySpriteBatch;
                }
                else
                {
                    return Parent.DrawC.MySpriteBatch;
                }
            }

            set
            {
                Parent.DrawC.Screen.CreateSharedSpriteBatch(eff);
                mySpriteBatch = value;
            }
        }

    }
}
