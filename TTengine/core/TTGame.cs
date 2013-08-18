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
using TTengine.Util;
using TTengine.Modifiers;
using TTMusicEngine;
using TTMusicEngine.Soundevents;

namespace TTengine.Core
{
    public abstract class TTGame: Game
    {
        // list of configurable paramaters
        public bool IsFullScreen = false;
        public int MyWindowWidth = 1024;
        public int MyWindowHeight = 768;

        /// <summary>
        /// Root gamelet
        /// </summary>
        public Gamelet Root;
        
        /// <summary>
        /// The currently running (single) instance of TTGame
        /// </summary>
        public static TTGame Instance = null;

        public GraphicsDeviceManager graphics;
        public Screenlet Screen;
        public MusicEngine musicEngine;        

        public TTGame()
        {
            Instance = this;
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            // call user's custom init to set/change parameters
            InitTTGame();
            graphics.IsFullScreen = IsFullScreen;
            graphics.PreferredBackBufferHeight = MyWindowHeight;
            graphics.PreferredBackBufferWidth = MyWindowWidth;
        }

        /// <summary>
        /// Initialize the configurable parameters in TTGame
        /// </summary>
        protected abstract void InitTTGame();

        /// <summary>
        /// Initialize content for this TTGame (called from Game.Initialize())
        /// </summary>
        protected abstract void InitTTContent();

        /// <summary>
        /// Load content for this TTGame (called from Game.LoadContent())
        /// </summary>
        protected abstract void LoadTTContent();

        protected override void Initialize()
        {
            TTengineMaster.Create(this);

            // open the TTMusicEngine
            musicEngine = MusicEngine.GetInstance();
            musicEngine.AudioPath = "Content";
            if (!musicEngine.Initialize())
                throw new Exception(musicEngine.StatusMsg);

            // from here on, main screen
            Screen = new Screenlet(MyWindowWidth, MyWindowHeight);
            TTengineMaster.ActiveScreen = Screen;
            Root = new FixedTimestepPhysics();
            Root.Add(Screen);

            // user's content init
            InitTTContent();

            // finally call base to enumnerate all (gfx) Game components to init
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadTTContent();
            Root.Init();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            TTengineMaster.Update(gameTime, Root);

            // update any other XNA components
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            TTengineMaster.Draw(gameTime, Root);

            // then draw other (if any) XNA game components on the screen
            base.Draw(gameTime);
        }

    }
}
