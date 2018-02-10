// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;
using Microsoft.Xna.Framework.Graphics;
using Artemis;

namespace TTengineTest
{
    /// <summary></summary>
    class TestCrtEffect : Test
    {

        public TestCrtEffect(): base()
        {
            this.BackgroundColor = Color.Black;    
        }

        public override void BuildAll()
        {
            var chan = CreateCrtChannel(New(), Color.Black, 960, 720);
            var p = GetFxParameters(chan);

            // set a script for shader params
            AddFunctionScript(chan,
                (ctx, v) => {
                    p["warpX"].SetValue((float)v);
                    p["warpY"].SetValue((float)v);
                },
                new SineFunction { Amplitude = 0.6, Offset = 0.061, Frequency = 0.3 }
            );

            // fill the channel with content
            using (BuildTo(chan))
            {
                var t = new TestMultiChannels(); // TestSphereCollision();
                t.BuildLike(this);
                t.BuildAll();
            }
        }

    }
}
