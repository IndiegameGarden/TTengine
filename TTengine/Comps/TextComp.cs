// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using Artemis.Interface;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;

namespace TTengine.Comps
{
    public class TextComp: IComponent
    {
        public SpriteFont Font;

        public string Text = "";

        public TextComp(string text, string fontName = "TTDefaultFont" )
        {
            this.Text = text;
            this.Font = TTGame.Instance.Content.Load<SpriteFont>(fontName);
        }
    }
}
