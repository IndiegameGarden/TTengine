using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTengine.Comps;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 3)]
    public class BlinkSystem : EntityComponentProcessingSystem<BlinkComp>
    {
        // TODO: check if non-active entities are also called in this process method. For all systems.
        public override void Process(Entity entity, BlinkComp bcomp)
        {
            double t = bcomp.SimTime % bcomp.TimePeriod;
            bool isVisible;
            if (t <= bcomp.TimeOn)
                isVisible = true;
            else
                isVisible = false;
            entity.GetComponent<DrawComp>().IsVisible = isVisible;
        }

    }
}
