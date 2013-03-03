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
            ConstructSpritelet();
        	DrawC.DrawColor = Color.Black * opacity ;
            this.height = height;
        }

        public override void OnNewParent()
        {
            base.OnNewParent();
            InitTextureBuffer();
        }

        void InitTextureBuffer()
        {
            Sprite.Texture = new Texture2D(DrawC.Screen.graphicsDevice, 1, 1);
            Sprite.Texture.SetData<Color>(new Color[] { Color.White });
            scaleVec = new Vector2(DrawC.Screen.WidthPixels * 2f, height * DrawC.Screen.HeightPixels);
            Sprite.Center = new Vector2(0.5f, 0f); // horiz-centered and vertically-aligned-top
            posX = DrawC.Screen.WidthPixels / 2f;
        }

        public override void OnDraw(ref DrawParams p)
        {
            if (Sprite.Texture != null)
            {
                Vector2 pos = DrawC.DrawPosition + DrawC.ToPixels(Motion.Position);
                pos.X += posX;
                DrawC.MySpriteBatch.Draw(Sprite.Texture, pos, null, DrawC.DrawColor,
                       Motion.RotateAbs, Sprite.DrawCenter, scaleVec * DrawC.DrawScale, SpriteEffects.None, DrawC.LayerDepth);
            }
        }

    }
}
