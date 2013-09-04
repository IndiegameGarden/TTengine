using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Systems;
using TTengine.Util;
using Artemis;
//using TTengine.Modifiers; // TODO
using TTMusicEngine;
using TTMusicEngine.Soundevents;

namespace TTengine.Core
{
    /// <summary>
    /// The Game Template. The base class for your game if you want to use TTengine.
    /// </summary>
    public abstract class TTGame: Game
    {
        /// <summary>If set true, starts the TTMusicEngine</summary>
        public bool IsMusicEngine = false;

        /// <summary>The currently running (single) instance of TTGame</summary>
        public static TTGame Instance = null;

        public GraphicsDeviceManager GraphicsMgr;

        public MusicEngine MusicEngine;

        /// <summary>The Artemis entity world that is currently used for building/creating new Entities in</summary>
        public EntityWorld BuildWorld;

        public Entity BuildScreen;

        public ScreenComp DrawScreen;

        public ChannelManager ChannelMgr ;

        public TTGame()
        {
            Instance = this;

            // XNA related init that needs to be in constructor (or at least before Initialize())
            GraphicsMgr = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
#if DEBUG
            GraphicsMgr.SynchronizeWithVerticalRetrace = false;
#else
            Graphics.SynchronizeWithVerticalRetrace = true;
#endif
        }

        protected override void Initialize()
        {
            ChannelMgr = new ChannelManager(this);

            // the TTMusicEngine
            if (IsMusicEngine)
            {
                MusicEngine = MusicEngine.GetInstance();
                MusicEngine.AudioPath = "Content";
                if (!MusicEngine.Initialize())
                    throw new Exception(MusicEngine.StatusMsg);
            }

            // default channel
            var ch = ChannelMgr.CreateChannel();
            ChannelMgr.ZapTo(ch);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (Channel c in ChannelMgr.Channels)
            {
                if (!c.IsActive)
                    continue;
                c.World.Update();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // loop all active channels and draw them.
            foreach (Channel c in ChannelMgr.Channels)
            {
                if (!c.IsActive || !c.IsVisible)
                    continue;
                DrawScreen = c.Screen.GetComponent<ScreenComp>();
                c.World.Draw();
            }
            base.Draw(gameTime);
        }

    }
}
