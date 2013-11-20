using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;

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
            // create animated sprite from 4x4 sprite atlas bitmap
            var s = TTFactory.CreateAnimatedSpritelet("SmileyWalk",4,4);
            s.GetComponent<PositionComp>().Position2D = new Vector2(500f, 300f);
        }

    }
}
