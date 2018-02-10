// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;

using Artemis;
using TTengine.Core;
using TTengine.Comps;

//using Game1.Comps;
//using Game1.Levels;

namespace Game1
{
    /// <summary>
    /// Factory class that is specific to this Game and all its levels.
    /// </summary>
    public class Game1Factory: TTFactory
    {

        /// <summary>
        /// Create the player's ship Entity
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Entity CreateShip(Entity e)
        {
            CreateSprite(e, "ball");
            e.AddC(new ScaleComp(2.0));
            e.AddC(new PlayerInputComp());
            //e.AddC(new InputToMotionComp());
            return e;
        }

        public Entity CreateNovaBall(Entity e, float x, float y, int type=1)
        {
            var fx = CreateFx(New(), "Nova");
            CreateSprite(e, "supernova"+type);
            e.C<SpriteComp>().CenterToMiddle();
            var sc = new ScaleComp { Scale = 2, ScaleSpeed = 0.01 };
            e.AddC(sc);
            e.C<PositionComp>().Position = new Vector3(x, y, 0.5f);
            var rc = new RotateComp { RotateSpeed = 0.1 };
            e.AddC(rc);
            AddScript(e, (ctx) => { e.C<ScaleComp>().ScaleTarget *= 1.001; } );            
            e.C<DrawComp>().DrawScreen = fx.C<ScreenComp>(); // draw nova onto the Shader/FX screen
            return e;
        }

        /// <summary>
        /// create a ball Sprite that can be scaled
        /// </summary>
        /// <param name="radius">the relative size scaling, 1 is normal</param>
        /// <returns></returns>
        public Entity CreateBall(Entity e, double radius)
        {
            CreateSprite(e, "paul-hardman_circle-four");
            e.AddC(new ScaleComp(radius));
            return e;
        }

        public Entity CreateMovingText(Entity e, Vector2 pos, string text)
        {
            CreateText(e,text);
            e.C<PositionComp>().PositionXY = pos;
            e.C<DrawComp>().DrawColor = Color.Black;
            e.C<VelocityComp>().VelocityXY = 0.2f * new Vector2(RandomMath.RandomUnit() - 0.5f, RandomMath.RandomUnit() - 0.5f);
            e.C<ScaleComp>().Scale = 0.5;
            return e;
        }

        /// <summary>
        /// create a moving ball with given position and velocity
        /// </summary>
        /// <returns></returns>
        public Entity CreateMovingBall(Entity e, Vector2 pos, Vector2 velo)
        {
            var ball = CreateBall(e, RandomMath.RandomBetween(0.96f, 1.08f));

            // position and velocity set
            ball.C<PositionComp>().PositionXY = pos;
            ball.C<PositionComp>().Depth = RandomMath.RandomBetween(0.5f,0.6f); // random Z position
            ball.C<VelocityComp>().Velocity = new Vector3(velo.X,velo.Y,0f);
            return ball;
        }

        public Entity CreateMovingBall(Entity e, Vector3 pos, Vector2 velo)
        {
            return CreateMovingBall(e, new Vector2(pos.X, pos.Y), velo);
        }

        public Entity CreateRotatingBall(Entity e, Vector2 pos, Vector2 velo, double rotSpeed)
        {
            CreateMovingBall(e, pos, velo);
            e.C<ScaleComp>().Scale = 0.7;
            var rc = new RotateComp();
            rc.RotateSpeed = rotSpeed;
            e.AddC(rc);
            return e;
        }

        public Entity CreateHypnoScreen(Entity e)
        {
            CreateFx(e, "Hypno");
            AddScript(e, ScriptHypno );
            return e;
        }

        void ScriptHypno(ScriptComp ctx)
        {
            float z = 17f - 15f * (float)Math.Sin(MathHelper.TwoPi * 0.03324 * ctx.SimTime);
            var effect = ctx.Entity.C<ScreenComp>().SpriteBatch.effect;
            effect.Parameters["Zoom"].SetValue(z);
            effect.Parameters["Time"].SetValue((float)ctx.SimTime);
        }

        public Entity CreateMandelbrotScreen(Entity e)
        {
            CreateFx(e, "MandelbrotJulia");
            AddScript(e, ScriptMandelbrotFx );
            return e;
        }

        void ScriptMandelbrotFx(ScriptComp ctx)
        {
            var effect = ctx.Entity.C<ScreenComp>().SpriteBatch.effect;
            effect.Parameters["Zoom"].SetValue((float)( 3 - ctx.SimTime/20.0 ));
        }

        public Entity CreateJuliaScreen(Entity e)
        {
            CreateFx(e, "MandelbrotJulia");
            var fx = e.C<ScreenComp>().SpriteBatch.effect;
            fx.CurrentTechnique = fx.Techniques[1]; // select Julia
            AddScript(e, ScriptJuliaFx);
            return e;
        }

        public Entity CreateJuliaFxSprite(Entity e)
        {            
            CreateFxSprite(e, "MandelbrotJulia");
            var fx = e.C<ScreenComp>().SpriteBatch.effect;
            fx.CurrentTechnique = fx.Techniques[1]; // select Julia
            AddScript(e, ScriptJuliaFx);
            return e;
        }

        void ScriptJuliaFx(ScriptComp ctx)
        {
            var effect = ctx.Entity.C<ScreenComp>().SpriteBatch.effect;
            var t = (float) ctx.SimTime;
            effect.Parameters["JuliaSeed"].SetValue( new Vector2(0.39f + t * 0.004f , -0.2f + t * 0.003f) );
        }

    }
}
