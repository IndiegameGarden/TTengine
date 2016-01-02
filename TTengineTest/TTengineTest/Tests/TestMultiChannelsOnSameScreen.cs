using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Render multiple channels on the same main screen</summary>
    class TestMultiChannelsOnSameScreen : Test
    {

        public override void Create()
        {
            var c1 = TestFactory.CreateChannel(BackgroundColor);
            TestFactory.BuildTo(c1);
            var t1 = new TestLinearMotion();
            t1.Create();

            var c2 = TestFactory.CreateChannel(BackgroundColor);
            TestFactory.BuildTo(c2);
            var t2 = new TestRotation();
            t2.Create();
        }

    }
}
