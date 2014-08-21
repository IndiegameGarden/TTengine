using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>
    /// Get pixel values and set pixels of a sprite bitmap
    /// </summary>
    class TestSpritePixelGetSet : Test
    {

        public TestSpritePixelGetSet()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            Factory.BallSprite = "amazing1.png";
            TTFactory.BuildChannel.Screen.SpriteBatch.samplerState = SamplerState.PointClamp; // set 'blocky' screen mode

            // sprite
            var velo = new Vector2(5f, 3f);
            var spr = Factory.CreateMovingBall(new Vector2(335f, 350f), velo);
            spr.GetComponent<ScaleComp>().Scale = 10f;

            // add script to sprite
            TTFactory.AddScript(spr, ChangePixelsRandomlyScript);

        }

        /// <summary>
        /// pixel modification code is here, called every update cycle
        /// </summary>
        /// <param name="context"></param>
        void ChangePixelsRandomlyScript(ScriptContext context)
        {
            // pick a random pixel
            var sc = context.Entity.GetComponent<SpriteComp>();
            int x = RandomMath.RandomIntBetween(0, (int)sc.Width);
            int y = RandomMath.RandomIntBetween(0, (int)sc.Height);
            Color c = sc.GetPixel(x, y);

            // change its color
            Color cNew = new Color((int)c.R - RandomMath.RandomIntBetween(95, 153),
                                    (int)c.G + RandomMath.RandomIntBetween(0, 2),
                                    (int)c.B + RandomMath.RandomIntBetween(98, 212), c.A);
            sc.SetPixel(x, y, cNew);
        }

    }
}
