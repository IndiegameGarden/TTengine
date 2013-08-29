// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;

namespace TTengine.Util
{
    /**
     * a debug text message that shows on screen
     */
    public class DebugMessage
    {
        protected string txt ;

        public DebugMessage Create(string initialText)
        {
            DebugMessage m = new DebugMessage(initialText);

            return m;
        }

        /// <summary>
        /// construct a box with initial text. Can be changed later with the Text propertyOrField.
        /// </summary>
        /// <param name="initialText">initial text to display</param>
        public DebugMessage(string initialText)
        {
            txt = initialText;
        }

        /// <summary>
        /// construct a box without text yet (empty)
        /// </summary>
        public DebugMessage()
        {
            txt = "";
        }

        /// <summary>
        /// get/set the text of this box
        /// </summary>
        public string Text
        {
            get
            {
                return txt;
            }
            set
            {
                txt = value;
            }
        }

        /*
        public void OnDraw(ref DrawParams ctx)
        {
            base.OnDraw(ref ctx);

            Vector2 origin = Vector2.Zero; // new Vector2(2f * txt.Length, 0f);
            DrawC.MySpriteBatch.DrawString(DrawC.Screen.DebugFont, txt, Motion.PositionAbsZoomedPixels, DrawC.DrawColor,
                                    Motion.RotateAbs, origin, Motion.ScaleAbs, SpriteEffects.None, DrawC.LayerDepth);
        }
         */

    }
}
