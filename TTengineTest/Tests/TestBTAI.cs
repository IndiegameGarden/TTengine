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

        public override void BuildAll()
        {
            var ball = CreateBall(New(),1);
            ball.C<PositionComp>().Position = new Vector3(300f, 300f, 0.5f);
            ball.C<VelocityComp>().VelocityXY = new Vector2(25f, 25f);

            // Behavior Tree AI
            BTAIComp ai = new BTAIComp();
            var randomWanderBehavior = new RandomWanderBehavior(1, 6);
            ai.rootNode = new PrioritySelector(randomWanderBehavior);
            ball.AddC(ai);
        }

    }
}
