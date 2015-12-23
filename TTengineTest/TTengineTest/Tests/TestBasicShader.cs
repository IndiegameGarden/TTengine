using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>
    /// Basic shader test that creates an EffectScreenlet and renders sprites to it
    /// </summary>
    class TestBasicShader : Test
    {

        public TestBasicShader()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            var fxScreen = TTFactory.CreateFxScreenlet("FixedColor");
            TTFactory.BuildTo(fxScreen);

            var t = new TestRotation();
            t.Create();
        }

    }
}
