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

        /// <summary>The current default draw-to screen, set by TTengine before any World.Draw() calls</summary>
        public ScreenComp DrawScreen;

        /// <summary>The one root World into which everything else lives</summary>
        public EntityWorld RootWorld;

        /// <summary>The one root Channel which renders everything (including other channels) to the display.</summary>
        public Entity RootChannel;

        /// <summary>
        /// lag is how much time (sec) the fixed timestep (gametime) updates lag to the actual time.
        /// This is used for controlling the World Updates and also for smooth interpolated rendering.
        /// </summary>
        public double TimeLag = 0.0;

        /// <summary>When true, loop time profiling using below CountingTimers is enabled.</summary>
        public bool IsProfiling;

        public CountingTimer TimerUpdate = new CountingTimer();

        public CountingTimer TimerDraw = new CountingTimer();

        private SpriteBatch rootSpriteBatch;
        private Rectangle renderRect;

        /// <summary>
        /// Constructor
        /// </summary>
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
            renderRect = new Rectangle(0, 0, myWindowWidth, myWindowHeight);
            GraphicsMgr.PreferredBackBufferWidth = myWindowWidth;
            GraphicsMgr.PreferredBackBufferHeight = myWindowHeight;
            GraphicsMgr.IsFullScreen = false;
            Window.IsBorderless = true;
        }

        protected override void Initialize()
        {
            RootWorld = new EntityWorld();
            RootWorld.InitializeAll(true);
            TTFactory.BuildTo(RootWorld);
            RootChannel = TTFactory.CreateChannel(Color.CornflowerBlue,true);
            TTFactory.BuildTo(RootChannel);

            // the TTMusicEngine
            if (IsAudio)
            {
                AudioEngine = MusicEngine.GetInstance();
                AudioEngine.AudioPath = "Content";
                if (!AudioEngine.Initialize())
                    throw new Exception(AudioEngine.StatusMsg);
            }

            rootSpriteBatch = new SpriteBatch(GraphicsDevice);            

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
            DrawScreen = RootChannel.GetComponent<ScreenComp>(); // initial set of the current DrawScreen. Other Systems may tweak this.
            GraphicsDevice.SetRenderTarget(null);   // note: this clears the render buffer also.
            RootWorld.Draw();   // draw world(s) into the RootChannel buffer
            //GraphicsDevice.SetRenderTarget(null);
            rootSpriteBatch.Begin();    // draw the RootChannel onto the display itself. No clear needed: auto-fill entire display.
            //rootSpriteBatch.Draw(DrawScreen.RenderTarget,null,renderRect);
            rootSpriteBatch.End();
            base.Draw(gameTime);
            if (IsProfiling)
            {
                TimerDraw.Update();
                TimerDraw.Stop();
            }
        }

    }
}
