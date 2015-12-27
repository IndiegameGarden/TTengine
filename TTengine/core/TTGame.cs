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
        /// <summary>If set true, starts the TTMusicEngine and AudioSystem</summary>
        protected bool IsAudio = false;

        /// <summary>The currently running (single) instance of TTGame</summary>
        public static TTGame Instance = null;

        public GraphicsDeviceManager GraphicsMgr;

        /// <summary>The audio/music engine, or null if none initialized</summary>
        public MusicEngine AudioEngine;

        /// <summary>The main (root) screen that all graphics are eventually rendered to</summary>
        public ScreenComp DrawScreen;

        /// <summary>The default (root) World into which everything else lives</summary>
        public EntityWorld World;

        /// <summary>
        /// lag is how much time (sec) the fixed timestep (gametime) updates lag to the actual time.
        /// This is used for controlling the World Updates and for interpolated rendering.
        /// </summary>
        public double TimeLag = 0.0;

        /// <summary>
        /// Time (sec) of last total update loop
        /// </summary>
        public double TimeUpdate = 0.0;

        public TTGame()
        {
            Instance = this;

            // XNA related init that needs to be in constructor (or at least before Initialize())
            GraphicsMgr = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false; // handle own fixed timesteps
            Content.RootDirectory = "Content";
#if DEBUG
            GraphicsMgr.SynchronizeWithVerticalRetrace = false; // FPS: as fast as possible
#else
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
            World = new EntityWorld();
            World.InitializeAll(true);
            TTFactory.BuildWorld = World;
            DrawScreen = new ScreenComp(false);

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
            DateTime t0 = FastDateTime.Now;
            double dt = TargetElapsedTime.TotalSeconds;
            // see http://gameprogrammingpatterns.com/game-loop.html
            TimeLag += gameTime.ElapsedGameTime.TotalSeconds;

            while (TimeLag >= dt)
            {
                World.Update(TargetElapsedTime.Ticks);
                TimeLag -= dt;
            }
            TimeUpdate = (FastDateTime.Now - t0).TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(DrawScreen.BackgroundColor);            
            World.Draw();
            base.Draw(gameTime);
        }

    }
}
