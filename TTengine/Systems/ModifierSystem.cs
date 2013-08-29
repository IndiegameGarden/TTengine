using System;
using System.Collections.Generic;
using Artemis;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using TTengine.Comps;
using TTengine.Core;
using TTengine.Modifiers;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 0)]
    public class ModifierSystem: EntityComponentProcessingSystem<ModifierComp>
    {
        protected double deltaTimeStep = 0;

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            // retrieve the delta time step once, before looping over all entities.
            deltaTimeStep = TimeSpan.FromTicks(this.EntityWorld.Delta).TotalSeconds;
            base.ProcessEntities(entities);
        }

        public override void Process(Artemis.Entity entity, ModifierComp modComp)
        {            
            // time keeping
            modComp.SimTime += deltaTimeStep;

            // iterate the modifiers, and applying each
            foreach (Modifier m in modComp.ModsList)
            {
                m.Execute(entity, modComp.SimTime);
            }

        }
    }
}
