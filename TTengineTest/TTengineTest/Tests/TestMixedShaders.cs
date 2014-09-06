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

        public TestMixedShaders()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var ch = TTFactory.BuildChannel;
            Factory.BallSprite = "paul-hardman_circle-four";

            var fxScreen = TTFactory.CreateFxScreenlet("Grayscale");
            TTFactory.BuildTo(fxScreen.GetComponent<ScreenComp>());
            Factory.CreateRotatingBall(new Vector2(100f, 100f), new Vector2(5f, 5f), 0.1);

            var fxScreen2 = TTFactory.CreateFxScreenlet("RandomColor");
            TTFactory.BuildTo(fxScreen2.GetComponent<ScreenComp>());
            Factory.CreateRotatingBall(new Vector2(500f, 400f), new Vector2(5f, 5f), -0.1);

            TTFactory.BuildTo(ch); // restore the normal BuildScreen
            Factory.CreateRotatingBall(new Vector2(900f, 100f), new Vector2(5f, 5f), 0.1);
        }

    }
}
