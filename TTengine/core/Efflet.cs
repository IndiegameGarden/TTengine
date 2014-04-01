// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /**
     * base class for (shader) effects that are applied to an entire screen, i.e. Screenlet.
     * Screenlet takes care of rendering the Efflets in order by calling OnDrawEfflet()
     * at the end of a Draw() cycle.
     * 
     * Subclasses of Efflet can use inside the shader these predefined variables:
     *   DrawColor - value of Gamelet.DrawColor as RGB plus the alpha value
     */
    public class Efflet: Gamelet
    {
        protected string fxFileName = "";
        protected Effect effect = null;
        protected SpriteBatch spriteBatch = null;
        protected EffectParameter drawColorParameter=null;

        public Efflet(string fxFileName)
        {
            this.fxFileName = fxFileName;
            DrawInfo = new DrawInfo();
            Add(DrawInfo);
            LoadEffect();
        }

        protected virtual void LoadEffect()
        {
            spriteBatch = new SpriteBatch(Screen.graphicsDevice);
            if (fxFileName != null)
            {
                effect = TTengineMaster.ActiveGame.Content.Load<Effect>(fxFileName);
                drawColorParameter = effect.Parameters["DrawColor"];
            }
            VertexShaderInit(effect);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            if (drawColorParameter != null) drawColorParameter.SetValue(DrawInfo.DrawColor.ToVector4() );
        }

        internal override void Draw(ref DrawParams p)
        {
            if (!Active) return;
            base.Draw(ref p);

            // add myself to list of efflets for post-process, in case I'm visible.
            if (Visible) Screen.effletsList.Add(this);
        }

        /// to override, called when eff should apply itself to a sourceBuffer, drawing to screen.RenderTarget as usual.
        public virtual void OnDrawEfflet(ref DrawParams p, RenderTarget2D sourceBuffer)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(sourceBuffer, Screen.ScreenRectangle, Color.White);
            spriteBatch.End();
        }

    }
}
