// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

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
    public class DarkeningHBar : Gamelet
    {
        Vector2 scaleVec;
        float height;
        float posX;

        public DarkeningHBar(float opacity, float height)
        {
            CreateSpritelet();
        	DrawInfo.DrawColor = Color.Black * opacity ;
            this.height = height;
        }

        public override void OnNewParent()
        {
            base.OnNewParent();
            InitTextureBuffer();
        }

        void InitTextureBuffer()
        {
            Sprite.Texture = new Texture2D(DrawInfo.Screen.graphicsDevice, 1, 1);
            Sprite.Texture.SetData<Color>(new Color[] { Color.White });
            scaleVec = new Vector2(DrawInfo.Screen.WidthPixels * 2f, height * DrawInfo.Screen.HeightPixels);
            Sprite.Center = new Vector2(0.5f, 0f); // horiz-centered and vertically-aligned-top
            posX = DrawInfo.Screen.WidthPixels / 2f;
        }

        protected override void OnDraw(ref DrawParams p)
        {
            if (Sprite.Texture != null)
            {
                Vector2 pos = DrawInfo.DrawPosition + DrawInfo.ToPixels(Motion.Position);
                pos.X += posX;
                DrawInfo.MySpriteBatch.Draw(Sprite.Texture, pos, null, DrawInfo.DrawColor,
                       Motion.RotateAbs, Sprite.DrawCenter, scaleVec * DrawInfo.DrawScale, SpriteEffects.None, DrawInfo.LayerDepth);
            }
        }

    }
}
