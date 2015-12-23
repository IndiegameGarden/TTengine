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
            var fxScreen = TTFactory.CreateFxScreenlet("Grayscale");
            TTFactory.BuildTo(fxScreen);

            var t = new TestRotation();
            t.Create();
        }

    }
}
