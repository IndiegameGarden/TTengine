using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>
    /// Basic shader test that creates an FxScreenlet and renders sprites to it
    /// </summary>
    class TestBasicShader : Test
    {

        public override void Create()
        {
            var fxScreen = TestFactory.CreateFxScreenlet(BackgroundColor,"FixedColor");
            BuildTo(fxScreen);

            var t = new TestRotation();
            t.Create();
        }

    }
}
