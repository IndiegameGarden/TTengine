// (c) 2010-2015 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

namespace TTengine.Systems
{
    #region Using statements

    using System;
    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;
    using Microsoft.Xna.Framework;
    using TTengine.Comps;

    #endregion

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.RotateSystem)]
    public class RotateSystem : EntityComponentProcessingSystem<RotateComp>
    {

        public override void Process(Entity entity, RotateComp rotComp)
        {
            if (rotComp.RotateSpeed > 0)
                rotComp.Rotate += rotComp.RotateSpeed * Dt;            
        }
    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.RotateToDrawrotateSystem)]
    public class RotateToDrawrotateSystem : EntityComponentProcessingSystem<RotateComp, DrawComp>
    {

        public override void Process(Entity entity, RotateComp rotComp, DrawComp drawComp)
        {
            drawComp.DrawRotation = (float)rotComp.RotateAbs;
        }

    }

}