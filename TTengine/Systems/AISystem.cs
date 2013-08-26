using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 2)]
    public class AISystem : EntityComponentProcessingSystem<AIComp>
    {
        private UpdateParams updParams = new UpdateParams();

        public override void Process(Entity entity, AIComp sc)
        {
            updParams.Dt = TimeSpan.FromTicks(this.EntityWorld.Delta).TotalSeconds;
            updParams.SimTime += updParams.Dt;
            updParams.Entity = entity;
            updParams.Comp = sc;
            // simulate all Behaviors
            foreach (Behavior b in sc.Behaviors)
            {
                b.OnUpdate(updParams);
            }
            // select the Behavior that triggers
            foreach (Behavior b in sc.Behaviors)
            {
                if (b.IsActive)
                {
                    b.OnExecute(updParams);
                    break;
                }
            }

        }

    }
}
