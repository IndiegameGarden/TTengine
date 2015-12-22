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
            GraphicsMgr.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            GraphicsMgr.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.IsBorderless = true;
        }

        public static new PXGame Instance;
        public Channel GameChannel;
        public PXLevel Level;

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // create a default Channel
            GameChannel = TTFactory.CreateChannel(Color.White, false);
            TTFactory.BuildTo(GameChannel);
            // PointClamp to let all grahics be sharp and blocky (non-interpolated pixels)
            GameChannel.Screen.SpriteBatch.samplerState = SamplerState.PointClamp;
            ChannelMgr.AddChannel(GameChannel);
            GameChannel.ZapTo(); 

            // add framerate counter
            FrameRateCounter.Create(Color.Black);

            base.LoadContent();
        }       

    }
}
