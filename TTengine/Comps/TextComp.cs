using System;
using System.Collections.Generic;

using System.Text;
using Artemis.Interface;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;

namespace TTengine.Comps
{
    public class TextComp: IComponent
    {
        public SpriteFont Font;

        public string Text = "";

        public TextComp(string text, string fontName = "Font1" )
        {
            this.Text = text;
            this.Font = TTGame.Instance.Content.Load<SpriteFont>(fontName);
        }
    }
}
