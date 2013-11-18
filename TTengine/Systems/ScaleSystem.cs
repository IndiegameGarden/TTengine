using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using TTengine.Comps;

using Artemis;
using Artemis.Manager;
using Artemis.Attributes;
using Artemis.System;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.ScaleSystem)]
    public class ScaleSystem : EntityComponentProcessingSystem<ScaleComp>
    {

        public override void Process(Entity entity, ScaleComp sc)
        {
            if (!sc.IsActive) return;

            // scaling logic towards target
            if (sc.IsActive && sc.ScaleSpeed > 0)
            {
                if (sc.Scale < sc.ScaleTarget)
                {
                    sc.Scale += sc.ScaleSpeed * (sc.ScaleTarget - sc.Scale); 
                    if (sc.Scale > sc.ScaleTarget)
                    {
                        sc.Scale = sc.ScaleTarget;
                    }
                }
                else if (sc.Scale > sc.ScaleTarget)
                {
                    sc.Scale += sc.ScaleSpeed * (sc.ScaleTarget - sc.Scale); 
                    if (sc.Scale < sc.ScaleTarget)
                    {
                        sc.Scale = sc.ScaleTarget;
                    }
                }
            }

            // set scale for drawing
            if (entity.HasComponent<DrawComp>())
            {
                entity.GetComponent<DrawComp>().DrawScale = (float) (sc.Scale * sc.ScaleModifier);
            }

            sc.ScaleModifier = 1; // the ModifierSystem may adapt this one later. Each round reset to 1.

        }

    }
}
