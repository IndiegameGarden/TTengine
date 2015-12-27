// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;
using TTengine.Util;

using Artemis;
using Artemis.Interface;
using TreeSharp;

namespace Game1
{
    /// <summary>
    /// Main game class acting as template for your own games based on TTengine.
    /// Modify this class directly to start coding your game.
    /// </summary>
    public class Game1 : TTGame
    {
        public Game1Factory Factory;

        public Game1()
        {
            IsAudio = false;    // set to true if audio is needed
        }

        protected override void Initialize()
        {
            Factory = Game1Factory.Instance;

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            DrawScreen.BackgroundColor = Color.White;

            // add framerate counter
            Game1Factory.CreateFrameRateCounter(Color.Black);

            // add several sprites             
            for (float x = 0.1f; x < 1.6f; x += 0.3f)
            {
                for (float y = 0.1f; y < 1f; y += 0.24f)
                {
                    var pos = new Vector2(x * DrawScreen.Width, y * DrawScreen.Height);
                    Factory.CreateHyperActiveBall(pos);
                    Factory.CreateMovingTextlet(pos,"This is the\nTTengine test. !@#$1234");
                    //break;
                }
                //break;
            }

            base.LoadContent();
        }                  
    }

    /// <summary>
    /// Factory to create new game-specific entities.
    /// For your own game, move it into a separate file.
    /// </summary>
    public class Game1Factory: TTFactory
    {
        private static Game1Factory _instance = null;
        private Game1 _game;

        private Game1Factory(Game1 game)
        {
            _game = game;
        }

        public static Game1Factory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Game1Factory(TTGame.Instance as Game1);
                return _instance as Game1Factory;
            }
        }

        protected Random rnd = new Random();

        /// <summary>
        /// create a ball Spritelet that can be scaled
        /// </summary>
        /// <param name="radius">the relative size scaling, 1 is normal</param>
        /// <returns></returns>
        public Entity CreateBall(double radius)
        {
            Entity e = CreateSpritelet("paul-hardman_circle-four");
            e.AddComponent(new ScaleComp(radius));
            return e;
        }

        /// <summary>
        /// create an active ball with given position and random velocity and some weird (AI) behaviors
        /// </summary>
        /// <returns></returns>
        public Entity CreateHyperActiveBall(Vector2 pos)
        {
            var ball = CreateBall(0.08f + 0.07f * (float)rnd.NextDouble());

            // position and velocity set
            ball.GetComponent<PositionComp>().Position = pos;
            ball.GetComponent<VelocityComp>().Velocity = 0.2f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);

            /*
            // duration of entity
            ball.AddComponent(new ExpiresComp(4 + 500 * rnd.NextDouble()));

            // Behavior Tree AI
            BTAIComp ai = new BTAIComp();
            var randomWanderBehavior = new RandomWanderBehavior(1, 6);
            ai.rootNode = new PrioritySelector(randomWanderBehavior);
            ball.AddComponent(ai);

            // Modifier to adapt scale
            TTFactory.AddModifier(ball, ScaleModifierScript);

            // another adapting scale with sine rhythm
            var s = new SineFunction();
            s.Frequency = 0.5;
            s.Amplitude = 0.25;
            s.Offset = 1;
            TTFactory.AddModifier(ball, ScaleModifierScript, s);

            // modifier to adapt rotation
            TTFactory.AddModifier(ball, RotateModifierScript);

            // set different time offset initially, per ball (for the modifiers)
            ball.GetComponent<ScriptComp>().SimTime = 10 * rnd.NextDouble();
            */

            ball.Refresh();
            return ball;

        }

        public Entity CreateMovingTextlet(Vector2 pos, string text)
        {
            var t = CreateTextlet(text);
            t.GetComponent<PositionComp>().Position = pos;
            t.GetComponent<DrawComp>().DrawColor = Color.Black;
            t.GetComponent<VelocityComp>().Velocity = 0.2f * new Vector2( (float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);
            t.GetComponent<ScaleComp>().Scale = 0.5;
            return t;
        }

        public void RotateModifierScript(ScriptContext ctx, double value)
        {
            ctx.Entity.GetComponent<DrawComp>().DrawRotation = (float)value;
        }
    }

}
