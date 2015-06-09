
using System;
using Microsoft.Xna.Framework;

using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using TTengine.Comps;
using PXengine.Comps;

namespace PXengine.Systems
{
    /// <summary>Scrolling of Screen.ZoomCenter system.</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.ScrollingSystem)]
    public class ScrollingSystem : EntityComponentProcessingSystem<ScrollingComp,DrawComp,PositionComp>
    {
        public override void Process(Entity entity, ScrollingComp sc, DrawComp dc, PositionComp pc)
        {
            sc.Scrolling.IsActive = true; // FIXME debug
            sc.Scrolling.OnUpdate(Dt);
            dc.DrawScreen.ZoomCenter = sc.Scrolling.Current;
            sc.Scrolling.Target = pc.Position;
        }
    }

}
