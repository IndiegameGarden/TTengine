using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.System;
using Artemis.Manager;
using Artemis.Attributes;
using TTengine.Comps;

namespace TTengine.Systems
{
    // TODO choose all layers of systems.
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 0)]
    public class SimSystem: EntityComponentProcessingSystem<SimComp>
    {
        protected double lastDt = 0;

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            // calc the last simulation timestep - to apply to all entities
            lastDt = TimeSpan.FromTicks(this.EntityWorld.Delta).TotalSeconds;
            base.ProcessEntities(entities);
        }

        public override void Process(Entity entity, SimComp sc)
        {
            if (sc.IsActive)
                sc.SimTime += lastDt;
        }
    }
}
