// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Artemis;
using TTengine.Comps;
using TTengine.Util;
using TTMusicEngine;

namespace TTengine.Core
{
    /// <summary>
    /// The Game Template. The base class for your game if you want to use TTengine.
    /// </summary>
    public abstract class TTGame: Game
    {
        /// <summary>The currently running (single) instance of TTGame</summary>
        public static TTGame Instance;

        /// <summary>If set to true in Game's constructor, starts both the TTMusicEngine and AudioSystem</summary>
        protected bool IsAudio = false;

        /// <summary>The XNA GraphicsDeviceManager for this Game</summary>
        public GraphicsDeviceManager GraphicsMgr;

        /// <summary>The audio/music engine, or null if none initialized</summary>
        public MusicEngine AudioEngine;

        /// <summary>The one root World into which everything else lives, including the MainChannel</summary>
        public EntityWorld RootWorld;

        /// <summary>Root screen where the MainChannel is drawn to.</summary>
        public ScreenComp RootScreen;

        /// <summary>
        /// lag is how much time (sec) the fixed timestep (gametime) updates lag to the actual time.
        /// This is used for controlling the World Updates and also for smooth interpolated rendering.
        /// </summary>
        public double TimeLag = 0.0;

        /// <summary>
        /// total Game Time in seconds
        /// </summary>
        public double GameTime = 0.0;

        /// <summary>When true, loop time profiling using below CountingTimers is enabled.</summary>
        public bool IsProfiling = false;

        /// <summary>Timer used for profiling: recording duration of total Update() cycle</summary>
        public CountingTimer ProfilingTimerUpdate = new CountingTimer();

        /// <summary>Timer used for profiling: recording duration of total Draw() cycle</summary>
        public CountingTimer ProfilingTimerDraw = new CountingTimer();

        /// <summary>The factory for creating the default TTGame Entities with</summary>
        internal static TTFactory Factory;

        /// <summary>
        /// Constructor
        /// </summary>
        public TTGame()
        {
            Instance = this;

            // XNA related init that needs to be in constructor (or at least before Initialize())
            GraphicsMgr = new GraphicsDeviceManager(this);
            GraphicsMgr.GraphicsProfile = GraphicsProfile.HiDef;
            IsFixedTimeStep = false; // handle own fixed timesteps in Update() code
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
            Factory = new TTFactory();

            // make root world and build to it
            RootWorld = new EntityWorld();
            RootWorld.InitializeAll(true);
            Factory.BuildTo(RootWorld);

            // make root screen and build to it
            RootScreen = new ScreenComp(false, 0, 0);
            Factory.BuildTo(RootScreen);

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

        protected override void Update(GameTime gameTime)
        {
            if (IsProfiling)
            {
                ProfilingTimerUpdate.Start();
                ProfilingTimerUpdate.CountUp();
            }
            this.GameTime = gameTime.TotalGameTime.TotalSeconds;
            double dt = TargetElapsedTime.TotalSeconds;
            // see http://gameprogrammingpatterns.com/game-loop.html
            TimeLag += gameTime.ElapsedGameTime.TotalSeconds;

            while (TimeLag >= dt)
            {
                RootWorld.Update(TargetElapsedTime);
                this.GameTime += dt;
                TimeLag -= dt;
            }
            base.Update(gameTime);
            if (IsProfiling)
            {
                ProfilingTimerUpdate.Update();
                ProfilingTimerUpdate.Stop();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsProfiling)
            {
                ProfilingTimerDraw.Start();
                ProfilingTimerDraw.CountUp();
            }
            this.GameTime = gameTime.TotalGameTime.TotalSeconds;
            RootScreen.SpriteBatch.BeginParameterized();
            RootWorld.Draw();   // draw world including all sub-worlds/sub-channels
            GraphicsDevice.SetRenderTarget(null);
            RootScreen.SpriteBatch.End();
            base.Draw(gameTime);
            if (IsProfiling)
            {
                ProfilingTimerDraw.Update();
                ProfilingTimerDraw.Stop();
            }
        }

    }
}
