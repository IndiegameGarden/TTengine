using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;
using TTengine.Comps;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestSpriteField : Test
    {
        public TestSpriteField()
            : base()        
        {
            BackgroundColor = Color.Black;
        }

        public override void Create()
        {
            var e = TTFactory.CreateSpriteField("test-spritefield.png", "red-circle");   // TODO white circle
            e.AddComponent(new VelocityComp());
            e.Refresh();

            e.GetComponent<VelocityComp>().Velocity2D = new Vector2(5f, 5f);

            SpriteFieldComp sfc = e.GetComponent<SpriteFieldComp>();
            sfc.NumberSpritesX = 10;
            sfc.NumberSpritesY = 5;
        }

    }
}
