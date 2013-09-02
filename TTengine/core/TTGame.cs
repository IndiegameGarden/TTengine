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

        /// <summary>The Artemis entity world</summary>
        public EntityWorld ActiveWorld;

        public Entity ActiveScreen;

        public ChannelManager ChannelMgr ;

        List<TTSpriteBatch> spriteBatchesActive = new List<TTSpriteBatch>();

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
                ActiveScreen = c.Screen;
                ActiveWorld = c.World;
                ActiveWorld.Update();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(ActiveScreen.GetComponent<ScreenletComp>().BackgroundColor);

            // loop all active channels and draw them.
            foreach (Channel c in ChannelMgr.Channels)
            {
                if (!c.IsActive)
                    continue;
                ActiveScreen = c.Screen;
                var sc = ActiveScreen.GetComponent<ScreenletComp>();
                ActiveWorld = c.World;

                spriteBatchesActive.Clear();

                this.GraphicsDevice.SetRenderTarget(sc.RenderTarget);
                UseSharedSpriteBatch(sc.SpriteBatch);
                ActiveWorld.Draw();
                sc.SpriteBatch.End();

                // close all remaining open effect-related spriteBatches
                foreach (SpriteBatch sb in spriteBatchesActive)
                    sb.End();
                spriteBatchesActive.Clear();

                // render
                List<ScreenletSystem> l = ActiveWorld.SystemManager.GetSystems<ScreenletSystem>();
                foreach (ScreenletSystem s in l)
                {
                    s.Process();
                }

                this.GraphicsDevice.SetRenderTarget(null);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// TODO something for a Mgr class?
        /// let the caller indicate that it wants to draw using the given shared SpriteBatch, which
        /// is not linked to any shader Effect. The SpriteBatch.Begin() method will be called if 
        /// needed here and also SpriteBatch.End() will be called by TTengine later after use.
        /// </summary>
        /// <param name="spb">spritebatch to request use of</param>
        internal void UseSharedSpriteBatch(TTSpriteBatch spb)
        {
            if (!spriteBatchesActive.Contains(spb))
            {
                spb.BeginParameterized();
                spriteBatchesActive.Add(spb);
            }
        }


    }
}
