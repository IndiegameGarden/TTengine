using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Artemis;
using Artemis.System;
using Artemis.Manager;
using Artemis.Attributes;
using PXengine.Comps;
using TTengine.Comps;

namespace PXengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.ThingSystem)]
    public class ThingSystem : EntityComponentProcessingSystem<ThingComp,PositionComp>
    {
        public override void Process(Entity entity, ThingComp tc, PositionComp pc)
        {
        }
    }
}
