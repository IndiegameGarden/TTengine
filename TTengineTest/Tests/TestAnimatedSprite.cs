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
    class TestAnimatedSprite : Test
    {
        public override void BuildAll()
        {
            // create animated sprite from 4x4 sprite atlas bitmap
            var s1 = CreateAnimatedSprite(New(), "SmileyWalk",4,4,AnimationType.NORMAL);
            s1.C<PositionComp>().PositionXY = new Vector2(200f, 300f);
            s1.C<AnimatedSpriteComp>().SlowdownFactor = 2;

            var s2 = CreateAnimatedSprite(New(), "SmileyWalk", 4, 4, AnimationType.REVERSE);
            s2.C<PositionComp>().PositionXY = new Vector2(400f, 300f);
            s2.C<AnimatedSpriteComp>().SlowdownFactor = 3;

            var s3 = CreateAnimatedSprite(New(), "SmileyWalk", 4, 4, AnimationType.NORMAL);
            s3.C<PositionComp>().PositionXY = new Vector2(600f, 300f);
            s3.C<AnimatedSpriteComp>().MinFrame = 4;
            s3.C<AnimatedSpriteComp>().MaxFrame = 12;
            s3.C<AnimatedSpriteComp>().SlowdownFactor = 2;

            var s4 = CreateAnimatedSprite(New(), "SmileyWalk", 4, 4, AnimationType.PINGPONG);
            s4.C<PositionComp>().PositionXY = new Vector2(200f, 400f);
            s4.C<AnimatedSpriteComp>().MinFrame = 4;
            s4.C<AnimatedSpriteComp>().MaxFrame = 12;
            s4.C<AnimatedSpriteComp>().SlowdownFactor = 4;

            var s5 = CreateAnimatedSprite(New(), "SmileyWalk", 4, 4, AnimationType.NORMAL);
            s5.C<PositionComp>().PositionXY = new Vector2(400f, 400f);
        }

    }
}
