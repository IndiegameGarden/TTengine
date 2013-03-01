using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// DrawInfo used for an Efflet
    /// </summary>
    public class EffectDrawInfo: DrawInfo
    {
        // override with custom spritebatch selection based on effect, delegating to the EffectSpriteInfo function for this
        public override TTSpriteBatch MySpriteBatch
        {
            get
            {
                return (Parent.Sprite as EffectSpriteInfo).MySpriteBatch;
            }

            set
            {
                (Parent.Sprite as EffectSpriteInfo).MySpriteBatch = value;
            }
        }

    }
}
