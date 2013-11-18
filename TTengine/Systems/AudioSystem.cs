using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using TTengine.Comps;
using TTMusicEngine;

using Artemis;
using Artemis.Manager;
using Artemis.Attributes;
using Artemis.System;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    public class AudioSystem : EntityComponentProcessingSystem<AudioComp>
    {
        double dt = 0;
        RenderParams rp = new RenderParams();
        MusicEngine musicEngine;

        protected override void Begin()
        {
            dt = TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
            musicEngine = TTGame.Instance.MusicEngine;
        }

        public override void Process(Entity entity, AudioComp ac)
        {
            if (!ac.IsActive) return;
            ac.UpdateComp(dt);
            rp.Time = ac.SimTime;
            rp.Ampl = ac.Ampl;
            musicEngine.Render(ac.AudioScript, rp);
        }

    }

}
