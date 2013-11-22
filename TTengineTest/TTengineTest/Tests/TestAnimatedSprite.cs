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
            var s1 = TTFactory.CreateAnimatedSpritelet("SmileyWalk",4,4,AnimationType.NORMAL);
            s1.GetComponent<PositionComp>().Position2D = new Vector2(200f, 300f);
            s1.GetComponent<AnimatedSpriteComp>().SlowdownFactor = 2;

            var s2 = TTFactory.CreateAnimatedSpritelet("SmileyWalk", 4, 4, AnimationType.REVERSE);
            s2.GetComponent<PositionComp>().Position2D = new Vector2(400f, 300f);
            s2.GetComponent<AnimatedSpriteComp>().SlowdownFactor = 3;

            var s3 = TTFactory.CreateAnimatedSpritelet("SmileyWalk", 4, 4, AnimationType.NORMAL);
            s3.GetComponent<PositionComp>().Position2D = new Vector2(600f, 300f);
            s3.GetComponent<AnimatedSpriteComp>().MinFrame = 4;
            s3.GetComponent<AnimatedSpriteComp>().MaxFrame = 12;
            s3.GetComponent<AnimatedSpriteComp>().SlowdownFactor = 2;

            var s4 = TTFactory.CreateAnimatedSpritelet("SmileyWalk", 4, 4, AnimationType.PINGPONG);
            s4.GetComponent<PositionComp>().Position2D = new Vector2(200f, 400f);
            s4.GetComponent<AnimatedSpriteComp>().MinFrame = 4;
            s4.GetComponent<AnimatedSpriteComp>().MaxFrame = 12;
            s4.GetComponent<AnimatedSpriteComp>().SlowdownFactor = 4;

            var s5 = TTFactory.CreateAnimatedSpritelet("SmileyWalk", 4, 4, AnimationType.NORMAL);
            s5.GetComponent<PositionComp>().Position2D = new Vector2(400f, 400f);
        }

    }
}
