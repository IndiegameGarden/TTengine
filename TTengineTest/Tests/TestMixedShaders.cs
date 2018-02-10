// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;
using Microsoft.Xna.Framework.Graphics;

namespace TTengineTest
{
    /// <summary>
    /// Shader test showing sprites that use different shader fx, including no-shader.
    /// </summary>
    public class TestMixedShaders : Test
    {

        public override void BuildAll()
        {
            BallSprite = "paul-hardman_circle-four";

            var fx1 = CreateFx(New(), "Grayscale");
            using (BuildTo(fx1))
                CreateRotatingScalingBall(New(), pos: new Vector2(100f, 100f), velo: new Vector2(5f, 5f), rotSpeed: 0.1);

            var fx2 = CreateFx(New(), "RandomColor");
            using (BuildTo(fx2))
                CreateRotatingScalingBall(New(), new Vector2(500f, 400f), new Vector2(5f, 5f), 0.1);

            var fx3 = CreateFx(New(), "FixedColor");
            using (BuildTo(fx3))
                CreateRotatingScalingBall(New(), new Vector2(1100f, 300f), new Vector2(1f, -2f), -0.1);

            var fx4 = CreateFx(New(), "Bloom1");
            using (BuildTo(fx4))
                CreateRotatingScalingBall(New(), new Vector2(900f, 680f), new Vector2(-1f, -1f), 0.1);

            // no shader effect
            CreateRotatingScalingBall(New(), new Vector2(900f, 100f), new Vector2(5f, 5f), -0.1);
        }

    }
}
