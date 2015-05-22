using System;
using System.Collections.Generic;
using System.Text;

using TTengine.Core;
using TTMusicEngine.Soundevents;

namespace TTengine.Comps
{
    /// <summary>
    /// Enables audio/music/soundeffect playing by an Entity, via the TTMusicEngine
    /// </summary>
    public class AudioComp: Comp
    {
        /// <summary>Audio playing time used for audio script rendering. Holds when audio paused.</summary>
        public double SimTime = 0.0;

        /// <summary>The audio script to play in TTMusicEngine format</summary>
        public SoundEvent AudioScript = null;

        /// <summary>The relative amplitude (0...1) to play the script with</summary>
        public double Ampl = 1.0;

        /// <summary>If true, the audio script is paused</summary>
        public bool IsPaused = false;

        public AudioComp()
        {
        }

        public AudioComp(SoundEvent script)
        {
            this.AudioScript = script;
        }

    }
}
