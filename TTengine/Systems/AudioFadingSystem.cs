using System;
using System.Collections.Generic;

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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.AudioSystemDraw)]
    public class AudioFadingSystem : EntityComponentProcessingSystem<AudioFadingComp,AudioComp>
    {

        public override void Process(Entity entity, AudioFadingComp afc, AudioComp ac)
        {
            if (afc.IsFading)
            {
                if (ac.Ampl < afc.FadeTarget)
                {
                    ac.IsPaused = false;    // resume paused play (if any)
                    ac.Ampl += afc.FadeSpeed * Dt;
                    if (ac.Ampl > afc.FadeTarget)
                    {
                        ac.Ampl = afc.FadeTarget;
                        afc.IsFading = false;
                    }
                }
                else if (ac.Ampl > afc.FadeTarget)
                {
                    ac.Ampl -= afc.FadeSpeed * Dt;
                    if (ac.Ampl < afc.FadeTarget)
                    {
                        ac.Ampl = afc.FadeTarget;
                        afc.IsFading = false;
                        if (ac.Ampl == 0)   // if target reached and ampl is now zero,
                            ac.IsPaused = true; // pause the playing.
                    }
                }
            }
        }

    }

}
