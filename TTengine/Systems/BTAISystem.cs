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
    public class BTAISystem : EntityComponentProcessingSystem<AIComp>
    {
        private UpdateParams updParams = new UpdateParams();

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            // once per update-cycle, set timing in updParams
            updParams.Dt = TimeSpan.FromTicks(this.EntityWorld.Delta).TotalSeconds;
            updParams.SimTime += updParams.Dt;
            base.ProcessEntities(entities);
        }

        public override void Process(Entity entity, AIComp sc)
        {
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
