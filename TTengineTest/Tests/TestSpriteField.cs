// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

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
        public override void BuildAll()
        {
            var e = CreateSpriteField(New(),"amazing1.png", "tree");   // TODO white circle
            e.AddC(new VelocityComp());

            e.C<VelocityComp>().VelocityXY = new Vector2(5f, 5f);

            SpriteFieldComp sfc = e.C<SpriteFieldComp>();
            sfc.FieldPos = new Vector2(0f, 0f);
            sfc.NumberSpritesX = 43;
            sfc.NumberSpritesY = 33;
        }

    }
}
