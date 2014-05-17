using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Modifiers;
using Artemis.Interface;
using TTMusicEngine;
using TTMusicEngine.Soundevents;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestAudioBasics: Test
    {
        public TestAudioBasics()
            : base()        
        {
        }

        public override void Create()
        {
            TTFactory.CreateAudiolet(Test_Sample());
            //TTFactory.CreateAudiolet(Test_Repeat());
        }

        /// <summary>
        /// simple sample play test
        /// </summary>
        /// <returns></returns>
        public SoundEvent Test_Sample()
        {
            SoundEvent soundScript = new SoundEvent("Test_Sample");
            SoundEvent evDing = new SampleSoundEvent("ambient-echoing-ding.wav");
            soundScript.AddEvent(0, evDing);
            return soundScript;
        }

        /**
         * simple test of generic Repeat feature - on a SoundEvent. Not directly applied on SampleSoundEvent here.
         */
        public SoundEvent Test_Repeat()
        {
            SoundEvent soundScript = new SoundEvent("Test_Repeat");
            // try a once event
            SampleSoundEvent evDing = new SampleSoundEvent("ambient-echoing-ding.wav");
            evDing.Repeat = 1;
            soundScript.AddEvent(1, evDing);

            SampleSoundEvent evDing2 = new SampleSoundEvent(evDing);
            SoundEvent dingHolderEv = new SoundEvent();
            dingHolderEv.AddEvent(0, evDing2);
            dingHolderEv.Repeat = 10;
            soundScript.AddEvent(5, dingHolderEv);

            soundScript.UpdateDuration(60);
            return soundScript;
        }


    }
}
