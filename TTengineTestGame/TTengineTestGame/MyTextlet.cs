// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine;
 using TTengine.Core;

namespace TTengineTestGame
{
    public class MyTextlet : Gamelet
    {
        protected string text;
        protected SpriteFont spriteFont;

        public MyTextlet( string text)
        {
            this.text = text;
            DrawColor = Color.White;
        }

        protected override void OnInit()
        {
            spriteFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>("Font1");
        }

        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 pos = PositionAbsolute;
            Vector2 posPixels = pos * TTengineMaster.ActiveGame.GraphicsDevice.DisplayMode.Height;
            Screen.spriteBatch.DrawString(spriteFont, text, posPixels, DrawColor);
        }
    }
}
