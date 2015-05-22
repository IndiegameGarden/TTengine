using System;
using System.Collections.Generic;

using System.Text;

using TTengine.Comps;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis;

namespace TTengine.Systems
{
    // TODO consider a soft (faded) blink as well.
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.BlinkSystem)]
    public class BlinkSystem : EntityComponentProcessingSystem<BlinkComp>
    {
        double dt = 0;

        protected override void Begin()
        {
            dt = TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        // TODO: check if non-active entities are also called in this process method. For all systems.
        public override void Process(Entity entity, BlinkComp bc)
        {
            double tprev = bc.SimTime % bc.TimePeriod;
            bc.SimTime += dt;
            double t = bc.SimTime % bc.TimePeriod;
            if (t <= bc.TimeOn)
            {
                bc.isVisible = true;
                if (tprev > bc.TimeOn)  // Blinks On
                    entity.GetComponent<DrawComp>().IsVisible = true;
            }
            else
            {
                bc.isVisible = false;
                if (tprev <= bc.TimeOn) // Blinks Off
                    entity.GetComponent<DrawComp>().IsVisible = false;
            }            
        }

    }
}
