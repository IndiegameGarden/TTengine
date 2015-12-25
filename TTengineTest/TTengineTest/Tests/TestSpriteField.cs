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
        public override void Create()
        {
            var e = TestFactory.CreateSpriteField("amazing1.png", "tree");   // TODO white circle
            e.AddComponent(new VelocityComp());
            e.Refresh();

            e.GetComponent<VelocityComp>().Velocity2D = new Vector2(5f, 5f);

            SpriteFieldComp sfc = e.GetComponent<SpriteFieldComp>();
            sfc.FieldPos = new Vector2(0f, 0f);
            sfc.NumberSpritesX = 43;
            sfc.NumberSpritesY = 33;
        }

    }
}
