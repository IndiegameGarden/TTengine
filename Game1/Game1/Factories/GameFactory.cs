using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Artemis;
using Artemis.Interface;
using TreeSharp;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;

namespace Game1.Factories
{
    /// <summary>
    /// Factory to create new game-specific entities
    /// </summary>
    public class GameFactory
    {
        private static GameFactory _instance = null;
        private Game1 _game;

        private GameFactory(Game1 game)
        {
            _game = game;
        }

        public static GameFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameFactory(TTGame.Instance as Game1);
                return _instance as GameFactory;
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
            Entity e = TTFactory.CreateSpritelet("paul-hardman_circle-four");
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
            //ball.Motion.Rotate = (float)(Math.PI * 2 * rnd.NextDouble());                    
            //ball.Timing.StartTime = 10f * (float)rnd.NextDouble();

            // duration of entity
            ball.AddComponent(new ExpiresComp(4 + 500 * rnd.NextDouble()));

            // blink                    
            //ball.AddComponent(new BlinkComp(0.3+5*rnd.NextDouble(),0.4+0.4*rnd.NextDouble()));

            // Behavior Tree AI
            BTAIComp ai = new BTAIComp();
            var randomWanderBehavior = new RandomWanderBehavior(1, 6);
            ai.rootNode = new PrioritySelector(randomWanderBehavior);
            ball.AddComponent(ai);

            // Modifier to adapt scale
            var m = new Modifier(MyScaleModifier);
                //delegate(Entity entity){ entity.GetComponent<ScaleComp>().Scale = 0.5 + entity.GetComponent<PositionComp>().Position.X; }
                //);
            m.AttachTo(ball); 
            
            // another adapting scale with sine rhythm
            var s = new SineModifier(MyScaleModifier2);
            s.Frequency = 0.5;
            s.Amplitude = 0.25;
            s.Offset = 1;
            s.AttachTo(ball);            

            // modifier to adapt rotation
            var r = new Modifier(MyRotateModifier, ball.GetComponent<DrawComp>());
            r.AttachTo(ball);

            // set different time offset initially, per ball (for the modifiers)
            ball.GetComponent<ModifierComp>().SimTime = 10 * rnd.NextDouble();

            ball.Refresh();
            return ball;

        }

        public Entity CreateMovingTextlet(Vector2 pos, string text) {
            var t = TTFactory.CreateTextlet("TTengine! @#$1234");
            t.GetComponent<PositionComp>().Position = pos;
            t.GetComponent<DrawComp>().DrawColor = Color.Black;
            t.GetComponent<VelocityComp>().Velocity = 0.2f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);
            t.GetComponent<ScaleComp>().Scale = 0.5;
            return t;
        }

        public void MyScaleModifier(Entity entity, double value) {
            entity.GetComponent<ScaleComp>().ScaleModifier *= 0.5 + entity.GetComponent<PositionComp>().Position.X; 
        }

        public void MyScaleModifier2(Entity entity, double value)
        {
            entity.GetComponent<ScaleComp>().ScaleModifier *= value;
        }

        public void MyRotateModifier(IComponent drawComp, double value)
        {
            ((DrawComp)drawComp).DrawRotation = (float) value;
        }
    }
}
