using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Artemis;
using Artemis.Utils;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Systems;
using TTengine.Util;
using TTMusicEngine;
using TTMusicEngine.Soundevents;

namespace TTengine.Core
{
    /// <summary>
    /// The Game Template. The base class for your game if you want to use TTengine.
    /// </summary>
    public abstract class TTGame: Game
    {
        /// <summary>If set to true in Game's constructor, starts both the TTMusicEngine and AudioSystem</summary>
        protected bool IsAudio = false;

        /// <summary>The currently running (single) instance of TTGame</summary>
        public static TTGame Instance;

        /// <summary>The XNA GraphicsDeviceManager for this Game</summary>
        public GraphicsDeviceManager GraphicsMgr;

        /// <summary>The audio/music engine, or null if none initialized</summary>
        public MusicEngine AudioEngine;

        /// <summary>The one root World into which everything else lives, including the MainChannel</summary>
        public EntityWorld RootWorld;

        /// <summary>Root screen where the MainChannel is drawn to.</summary>
        public ScreenComp RootScreen;

        /// <summary>The main Channel is a scalable screen ('graphics window') and World in
        /// which all other entities, channels, levels etc exist. It lives inside the RootWorld and renders
        /// to the RootScreen.</summary>
        public Entity MainChannel;

        /// <summary>The Screen of the MainChannel.</summary>
        public ScreenComp MainChannelScreen;

        /// <summary>
        /// lag is how much time (sec) the fixed timestep (gametime) updates lag to the actual time.
        /// This is used for controlling the World Updates and also for smooth interpolated rendering.
        /// </summary>
        public double TimeLag = 0.0;

        /// <summary>When true, loop time profiling using below CountingTimers is enabled.</summary>
        public bool IsProfiling;

        public CountingTimer TimerUpdate = new CountingTimer();

        public CountingTimer TimerDraw = new CountingTimer();

        /// <summary>
        /// Constructor
        /// </summary>
        public TTGame()
        {
            Instance = this;

            // XNA related init that needs to be in constructor (or at least before Initialize())
            GraphicsMgr = new GraphicsDeviceManager(this);
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
            // make root world and build to it
            RootWorld = new EntityWorld();
            RootWorld.InitializeAll(true);
            TTFactory.BuildTo(RootWorld);

            // make root screen and build to it
            RootScreen = new ScreenComp(false, 0, 0);
            TTFactory.BuildTo(RootScreen);

            // make the MainChannel and build to it
            MainChannel = TTFactory.CreateChannel(Color.CornflowerBlue);
			MainChannelScreen = MainChannel.GetComponent<WorldComp>().Screen;
            TTFactory.BuildTo(MainChannel);

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
            RootScreen.SpriteBatch.BeginParameterized();
            RootWorld.Draw();   // draw world including all sub-worlds/sub-channels
            RootScreen.SpriteBatch.End();
            base.Draw(gameTime);
            if (IsProfiling)
            {
                TimerDraw.Update();
                TimerDraw.Stop();
            }
        }

    }
}
