// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary></summary>
    class TestGeom3D : Test
    {
        public TestGeom3D()
        {
            this.BackgroundColor = Color.Black;
        }

        public override void BuildAll()
        {
            this.BallSprite = "Op-art-circle_Marco-Braun";

            var s = AddScalable( CreateSphere(New(),new Vector3(BuildScreen.Center.X,BuildScreen.Center.Y,0f),250.0f) );
            AddFunctionScript(s, (ctx, v) =>
                {
                    s.C<ScaleComp>().Scale = v;
                }, new SineFunction { Amplitude = 0.12, Offset = 1.0, Frequency = 0.333 }
            );
            var pic = new PlayerInputComp();
            s.AddC(pic);
            AddScript(s, (ctx) =>
                {
                    s.C<PositionComp>().PositionXY += pic.Direction * (float)ctx.Dt * 250.0f;
                });

            // 2D ball follows the 3D one in (x,y)
            var b = CreateRotatingBall(New(), Vector2.Zero, Vector2.Zero, 0.05);
            AddScalable(b,2);            
            AddScript(b, (ctx) => { b.C<PositionComp>().Position = s.C<PositionComp>().Position; });
        }

    }
}
