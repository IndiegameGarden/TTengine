using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>
    /// Shader test with texture sampling, creates an EffectScreenlet and renders sprites to it
    /// </summary>
    class TestTextureSamplingShader : Test
    {

        public TestTextureSamplingShader()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var ch = TTFactory.BuildChannel;

            var fxScreen = TTFactory.CreateFxScreenlet("Grayscale");
            TTFactory.BuildTo(fxScreen.GetComponent<ScreenComp>());

            var t = new TestRotation();
            t.Create();

            TTFactory.BuildTo(ch); // restore the normal BuildScreen
        }

    }
}
