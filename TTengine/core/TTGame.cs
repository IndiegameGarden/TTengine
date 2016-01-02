using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Systems;
using TTengine.Util;
using Artemis;
using Artemis.Utils;
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
        /// <summary>If set true in constructor, starts the TTMusicEngine and AudioSystem</summary>
        protected bool IsAudio = false;

        /// <summary>The currently running (single) instance of TTGame</summary>
        public static TTGame Instance = null;

        public GraphicsDeviceManager GraphicsMgr;

        /// <summary>The audio/music engine, or null if none initialized</summary>
        public MusicEngine AudioEngine;

        /// <summary>The current draw-to screen, set by TTengine before World.Draw() calls</summary>
        public ScreenComp DrawScreen;

        public ScreenComp RootScreen;

        /// <summary>The default (root) World into which everything else lives</summary>
        public EntityWorld RootWorld;

        public Channel RootChannel;

        /// <summary>
        /// lag is how much time (sec) the fixed timestep (gametime) updates lag to the actual time.
        /// This is used for controlling the World Updates and for interpolated rendering.
        /// </summary>
        public double TimeLag = 0.0;

        /// <summary>When true, loop time profiling using below CountingTimers is enabled.</summary>
        public bool IsProfiling;

        public CountingTimer TimerUpdate = new CountingTimer();

        public CountingTimer TimerDraw = new CountingTimer();

        public TTGame()
        {
            Instance = this;

            // XNA related init that needs to be in constructor (or at least before Initialize())
            GraphicsMgr = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false; // handle own fixed timesteps
            Content.RootDirectory = "Content";
#if DEBUG
            IsProfiling = true;
            GraphicsMgr.SynchronizeWithVerticalRetrace = false; // FPS: as fast as possible
#else
            IsProfiling = false;
            GraphicsMgr.SynchronizeWithVerticalRetrace = true;
#endif
            int myWindowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int myWindowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            GraphicsMgr.PreferredBackBufferWidth = myWindowWidth;
            GraphicsMgr.PreferredBackBufferHeight = myWindowHeight;
            GraphicsMgr.IsFullScreen = false;
            Window.IsBorderless = true;
        }

        protected override void Initialize()
        {
            RootWorld = new EntityWorld();
            RootWorld.InitializeAll(true);
            TTFactory.BuildWorld = RootWorld;
            RootScreen = new ScreenComp(false);
            RootChannel = new Channel(RootWorld, RootScreen);
            var screenlet = TTFactory.CreateEntity();
            screenlet.AddComponent(RootScreen);
            screenlet.Refresh();

            // the TTMusicEngine
            if (IsAudio)
            {
                AudioEngine = MusicEngine.GetInstance();
                AudioEngine.AudioPath = "Content";
                if (!AudioEngine.Initialize())
                    throw new Exception(AudioEngine.StatusMsg);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsProfiling)
            {
                TimerUpdate.Start();
                TimerUpdate.CountUp();
            }
            double dt = TargetElapsedTime.TotalSeconds;
            // see http://gameprogrammingpatterns.com/game-loop.html
            TimeLag += gameTime.ElapsedGameTime.TotalSeconds;

            while (TimeLag >= dt)
            {
                RootWorld.Update(TargetElapsedTime);
                TimeLag -= dt;
            }
            base.Update(gameTime);
            if (IsProfiling)
            {
                TimerUpdate.Update();
                TimerUpdate.Stop();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsProfiling)
            {
                TimerDraw.Start();
                TimerDraw.CountUp();
            }
            DrawScreen = RootScreen; // initial set of the current DrawScreen. Other Systems may tweak this.
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(DrawScreen.BackgroundColor);            
            RootWorld.Draw();
            base.Draw(gameTime);
            if (IsProfiling)
            {
                TimerDraw.Update();
                TimerDraw.Stop();
            }
        }

    }
}
