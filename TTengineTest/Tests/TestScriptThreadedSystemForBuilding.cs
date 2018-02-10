// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;
using System.Threading;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Systems;
using TTengine.Modifiers;
using Artemis;
using Artemis.Interface;

namespace TTengineTest
{
    /// <summary>Test the ScriptThreadedSystem for basic entity-building operation</summary>
    class TestScriptThreadedSystemForBuilding : Test
    {

        public override void BuildAll()
        {
            // add regular content  (non-builder created)
            Test t = new TestSphereCollision();
            //Test t = new TestScaling();
            t.BuildLike(this);
            t.BuildAll();

            // add builder entity (test building in background thread)
            // this content is added while the 'game' plays
            var e = New();
            AddScript(e, new EntityScriptTriggersBuilding1(this) );
        }

        /// <summary>
        /// The script being added to the Entity, which triggers the build once at some point in time.
        /// </summary>
        class EntityScriptTriggersBuilding1: IScript
        {
            bool isDone = false;
            TestScriptThreadedSystemForBuilding testClass;

            public EntityScriptTriggersBuilding1(TestScriptThreadedSystemForBuilding testClass)
            {
                this.testClass = testClass;
            }

            public void OnUpdate(ScriptComp ctx)
            {
                // if enough time has passed, launch new builder job into the queue and
                // self-destruct this script.
                if (!isDone && ctx.SimTime > 5)
                {
                    var job = new ScriptJob(testClass.TestBuilderScript1, ctx.Entity);
                    ScriptThreadedSystem.AddToQueue(job);
                    isDone = true;
                }
            }

            public void OnDraw(ScriptComp ctx) {; }
        }

        /// <summary>
        /// Script to run in the background thread, building new entities at a steady pace.
        /// Each Entity once completed configured is released into the World using the 
        /// Factory.Finalize() method.
        /// </summary>
        void TestBuilderScript1(ScriptComp ctx)
        {
            BuildTo(Channel);
            BallSprite = "paul-hardman_circle-four";
            for (int i=0; i < 125; i++)
            {
                Vector2 pos = new Vector2(RandomMath.RandomBetween(0f,1400f), RandomMath.RandomBetween(0f,1000f));
                Vector2 vel = new Vector2(RandomMath.RandomBetween(-4f,4f), RandomMath.RandomBetween(-4f, 4f));
                // below: basis is a Disabled entity. It is only enabled using Finalize() once completely built.
                Entity e = CreateMovingBall(NewDisabled(), pos, vel, RandomMath.RandomBetween(0.1f,0.6f), 0.8f+i*0.0001f);
                Finalize(e); // release into the World!
                Thread.Sleep(30);
            }
        }

    }
}
