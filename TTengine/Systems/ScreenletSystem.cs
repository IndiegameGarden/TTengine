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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    public class ScreenletSystem : EntityComponentProcessingSystem<ScreenletComp>
    {

        public override void Process(Entity entity, ScreenletComp screen)
        {
            if (!screen.IsActive) return;
            // FIXME code here !?

        }

    }
}
