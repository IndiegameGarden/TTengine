using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using TTengine.Core;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestAnimatedSprite : Test
    {
        public TestAnimatedSprite()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            TTFactory.CreateAnimatedSpritelet("SmileyWalk");
        }

    }
}
