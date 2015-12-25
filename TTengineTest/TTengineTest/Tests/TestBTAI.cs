using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using TTengine.Behaviors;
using Artemis;
using Artemis.Interface;
using TreeSharp;

namespace TTengineTest
{
    /// <summary>
    /// Basic test of the Treesharp Behavior Tree AI system
    /// </summary>
    class TestBTAI : Test
    {

        public override void Create()
        {
            var ball = Factory.CreateBall(1);
            ball.GetComponent<PositionComp>().Position = new Vector2(300f, 300f);
            ball.GetComponent<VelocityComp>().Velocity2D = new Vector2(25f, 25f);

            // Behavior Tree AI
            BTAIComp ai = new BTAIComp();
            var randomWanderBehavior = new RandomWanderBehavior(1, 6);
            ai.rootNode = new PrioritySelector(randomWanderBehavior);
            ball.AddComponent(ai);
        }

    }
}
