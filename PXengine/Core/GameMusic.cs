using System;
using System.Collections.Generic;
using TTengine.Core;
using TTengine.Comps;
using Microsoft.Xna.Framework.Media;
using TTMusicEngine;
using TTMusicEngine.Soundevents;

namespace PXengine.Core
{
    public class GameMusic: IScript
    {
        float vol = 2f;
        SoundEvent soundScript;
        RenderParams rp = new RenderParams();

        public GameMusic()
        {
            //track = TTengineMaster.ActiveGame.Content.Load<Song>("A01");
            soundScript = new SoundEvent("GameMusic");
            SampleSoundEvent ev1 = new SampleSoundEvent("ductia.ogg"); 
            soundScript.AddEvent(2, ev1);
            soundScript.AddEvent(166, ev1);
            soundScript.AddEvent(332, ev1);
            soundScript.AddEvent(498, ev1);
            soundScript.AddEvent(664, ev1);
        }

        public float Volume
        {
            get
            {
                return vol;
            }
            set
            {
                vol = value;
                if (vol < 0f) vol = 0f;
                if (vol > 1f) vol = 1f;

                rp.Ampl = vol;
            }
        }

        public void OnUpdate(ScriptContext ctx)
        {
            rp.Time = ctx.SimTime; // gameTime.ElapsedGameTime.TotalSeconds;
            MusicEngine.GetInstance().Render(soundScript, rp);
        }

        public void OnDraw(ScriptContext ctx)
        {
        }

    }
}
