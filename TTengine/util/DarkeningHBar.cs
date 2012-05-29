// (c) 2010-2012 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;

namespace TTengine.Util
{
    /**
     * shows a framerate counter on screen (FPS) calculated
     * from timing of draw/upd calls.
     */
    public class DarkeningHBar : Spritelet
    {
        Vector2 scaleVec;
        float height;
        float posX;

        public DarkeningHBar(float opacity, float height)
        {
        	DrawInfo.DrawColor = Color.Black * opacity ;
            this.height = height;
        }

        protected override void OnNewParent()
        {
            base.OnNewParent();
            InitTextureBuffer();
        }

        void InitTextureBuffer()
        {
            Texture = new Texture2D(Screen.graphicsDevice, 1, 1);
            Texture.SetData<Color> (  new Color[] { Color.White } );
            scaleVec = new Vector2(Screen.WidthPixels, height * Screen.HeightPixels);
            posX = Screen.WidthPixels / 2f;
        }

        protected override void OnDraw(ref DrawParams p)
        {
            if (Texture != null)
            {
                Vector2 pos = Parent.DrawInfo.DrawPosition + ToPixels(Motion.Position);
                pos.X = posX;
                MySpriteBatch.Draw(Texture, pos, null, DrawInfo.DrawColor,
                       Motion.RotateAbs, DrawInfo.DrawCenter, scaleVec * DrawInfo.DrawScale, SpriteEffects.None, DrawInfo.LayerDepth);
            }
        }

    }
}
