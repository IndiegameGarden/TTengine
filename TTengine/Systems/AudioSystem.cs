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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.AudioSystem)]
    public class AudioSystem : EntityComponentProcessingSystem<AudioComp>
    {
        double dt = 0;
        RenderParams rp = new RenderParams();
        MusicEngine musicEngine = null;

        protected override bool CheckProcessing()
        {
            // disable this system if the MusicEngine is not enabled.
            if (!TTGame.Instance.IsMusicEngine)
                IsEnabled = false;
            
            return base.CheckProcessing();
        }

        protected override void Begin()
        {
            musicEngine = TTGame.Instance.MusicEngine;
            dt = TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
            musicEngine.Update(); // to be called once every frame
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
