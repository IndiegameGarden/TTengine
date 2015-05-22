using System;
using System.Collections.Generic;

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
            // scaling logic towards target
            if (sc.ScaleSpeed > 0)
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
        }

    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.ScaleToDrawscaleSystem)]
    public class ScaleToDrawscaleSystem : EntityComponentProcessingSystem<ScaleComp, DrawComp>
    {
        public override void Process(Entity entity, ScaleComp sc, DrawComp dc)
        {
            dc.DrawScale = (float)sc.ScaleAbs;
        }
    }
}
