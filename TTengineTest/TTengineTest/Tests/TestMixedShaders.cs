using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>
    /// Shader test showing sprites that use different shaders, including no-shader.
    /// </summary>
    class TestMixedShaders : Test
    {

        public override void Create()
        {
            Factory.BallSprite = "paul-hardman_circle-four";

            var fxScreen = TestFactory.CreateFxScreenlet(BackgroundColor,"Grayscale");
            BuildTo(fxScreen);
            Factory.CreateRotatingBall(new Vector2(100f, 100f), new Vector2(5f, 5f), 0.1);

            var fxScreen2 = TestFactory.CreateFxScreenlet(BackgroundColor, "RandomColor");
            BuildTo(fxScreen2);
            Factory.CreateRotatingBall(new Vector2(500f, 400f), new Vector2(5f, 5f), -0.1);

            BuildToDefault();
            Factory.CreateRotatingBall(new Vector2(900f, 100f), new Vector2(5f, 5f), 0.1);
        }

    }
}
