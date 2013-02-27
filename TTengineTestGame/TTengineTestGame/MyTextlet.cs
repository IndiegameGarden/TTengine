// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine;
 using TTengine.Core;

namespace TTengineTestGame
{
    public class MyTextlet : Drawlet
    {
        protected string text;
        protected SpriteFont spriteFont;

        public MyTextlet( string text)
        {
            this.text = text;
            DrawInfo.DrawColor = Color.White;
        }

        protected override void OnInit()
        {
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>("Font1");
        }

        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 pos = Motion.PositionAbs;
            Vector2 posPixels = pos * TTengineMaster.ActiveGame.GraphicsDevice.DisplayMode.Height;
            MySpriteBatch.DrawString(spriteFont, text, posPixels, DrawInfo.DrawColor);
        }
    }
}
