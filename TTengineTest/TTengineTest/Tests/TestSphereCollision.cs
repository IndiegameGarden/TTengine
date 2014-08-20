using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>
    /// Test the sphere-shape collision system
    /// </summary>
    class TestSphereCollision : Test
    {

        public TestSphereCollision()
            : base()        
        {
            BackgroundColor = Color.White;
        }

        public override void Create()
        {
            Factory.BallSprite = "red-circle";
            int BALLS_PER_ROW = 9;

            for (int i = 0; i < 35; i++)
            {
                float radius = RandomMath.RandomBetween(0.1f, 0.5f);
                var ball = Factory.CreateBall(radius);
                ball.GetComponent<PositionComp>().Position2D = new Vector2(150f * (i % BALLS_PER_ROW),
                                                                    200f * (float)Math.Floor((double)(i / BALLS_PER_ROW)));
                ball.GetComponent<VelocityComp>().Velocity2D = RandomMath.RandomDirection() * 30f;
                ball.AddComponent(new SphereShapeComp(radius * 250f));

                Modifier<Entity> m = new Modifier<Entity>(BallColorModifier, ball);
                m.AttachTo(ball);
            }
        }

        void BallColorModifier(Entity entity, double value)
        {
            var sc = entity.GetComponent<SphereShapeComp>();
            var dc = entity.GetComponent<DrawComp>();
            float c = Math.Max(1f - sc.Colliders.Count * 0.2f, 0f);
            dc.DrawColor = new Color(c,c,c);
        }


    }
}
