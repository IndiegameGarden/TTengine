// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;
using TTengine.Util;
using Artemis;
using Artemis.Interface;
using TreeSharp;

using PXengine.Core;

namespace PXengine.Core
{
    /// <summary>
    /// Pixie2 using TTengine-5
    /// </summary>
    public class PXGame : TTGame
    {
        public PXGame()
        {
            Instance = this;
            GraphicsMgr.IsFullScreen = false;
            IsMouseVisible = false;
        }

        public static new PXGame Instance;
        public PXLevel Level;
        public Entity  LevelScreen;

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LevelScreen = TTFactory.CreateScreenlet(Color.White,true,1024,768);
            // PointClamp to let all grahics be sharp and blocky (non-interpolated pixels)
            LevelScreen.GetComponent<ScreenComp>().SpriteBatch.samplerState = SamplerState.PointClamp;

            // add framerate counter
            // FIXME rework to TTfactory
            // FrameRateCounter.Create(Color.Black);

            base.LoadContent();
        }       

    }
}
