// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;

using Artemis;
using TTengine.Modifiers;

namespace TTengineTest
{
    /// <summary>
    /// Factory to create new test-specific entities
    /// </summary>
    public class TestFactory: TTFactory
    {
        /// <summary>Can change here the sprite name used for creating all Ball type entities</summary>
        public string BallSprite = "red-circle_frank-tschakert";

        /// <summary>
        /// create a ball Sprite that can be scaled
        /// </summary>
        /// <param name="radius">the relative size scaling, 1 is normal</param>
        /// <returns></returns>
        public Entity CreateBall(Entity e, double radius, float layerDepth = 0.5f)
        {
            CreateSprite(e,this.BallSprite);
            e.C<PositionComp>().Depth = layerDepth;
            e.C<SpriteComp>().CenterToMiddle();
            e.AddC(new ScaleComp(radius));
            return e;
        }

        /// <summary>
        /// create an active ball with given position and random velocity and some weird (AI) behaviors
        /// </summary>
        /// <returns></returns>
        public Entity CreateMovingBall(Entity e, Vector2 pos, Vector2 velo, double radius = 1.0, float layerDepth = 0.5f)
        {
            var ball = CreateBall(e,radius,layerDepth);

            // position and velocity set
            ball.C<PositionComp>().PositionXY = pos;
            ball.C<VelocityComp>().VelocityXY = velo;
            return ball;
        }

        public Entity CreateMovingBall(Entity e, Vector3 pos, Vector2 velo)
        {
            return CreateMovingBall(e, new Vector2(pos.X, pos.Y), velo);
        }

        public Entity CreateRotatingBall(Entity e, Vector2 pos, Vector2 velo, double rotSpeed)
        {
            var ball = CreateMovingBall(e, pos, velo);
            ball.C<ScaleComp>().Scale = 0.7;
            var rc = new RotateComp { RotateSpeed = rotSpeed };
            ball.AddC(rc);
            return ball;
        }

        public Entity CreateRotatingScalingBall(Entity e, Vector2 pos, Vector2 velo, double rotSpeed)
        {
            var ball = CreateRotatingBall(e, pos, velo, rotSpeed);
            AddFunctionScript(e, (sc, v) => { sc.Entity.C<ScaleComp>().Scale = v; }, new SineFunction { Offset = 0.7, Amplitude = 0.05} );
            return ball;
        }

        public Entity CreateText(Entity e, Vector2 pos, string text, Color col, float layerDepth = 0.5f)
        {
            CreateText(e,text);
            e.C<PositionComp>().PositionXY = pos;
            e.C<PositionComp>().Depth = layerDepth;
            e.C<DrawComp>().DrawColor = col;
            e.C<ScaleComp>().Scale = 0.8;
            return e;
        }

    }
}
