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
using Artemis;
//using TTengine.Modifiers; // TODO
using TTMusicEngine;
using TTMusicEngine.Soundevents;

namespace TTengine.Core
{
    public abstract class TTGame: Game
    {
        /// <summary>If set true, starts the TTMusicEngine</summary>
        public bool IsMusicEngine = false;

        /// <summary>The currently running (single) instance of TTGame</summary>
        public static TTGame Instance = null;

        public GraphicsDeviceManager GraphicsMgr;

        /// <summary>All the screens currently rendered by this Game during Draw()</summary>
        public List<Entity> Screens = new List<Entity>();

        public ScreenletComp ActiveScreen;
        
        public MusicEngine MusicEngine;

        /// <summary>The Artemis entity world</summary>
        public EntityWorld ActiveWorld;

        public TTGame()
        {
            Instance = this;

            // XNA related init
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
            // the TTMusicEngine
            if (IsMusicEngine)
            {
                MusicEngine = MusicEngine.GetInstance();
                MusicEngine.AudioPath = "Content";
                if (!MusicEngine.Initialize())
                    throw new Exception(MusicEngine.StatusMsg);
            }

            // main screen
            CreateScreenlet();
            base.Initialize();
        }

        protected Entity CreateScreenlet()
        {
            var sc = new ScreenletComp(false, GraphicsMgr.PreferredBackBufferWidth, GraphicsMgr.PreferredBackBufferHeight);
            Entity e = CreateScreenlet(sc);
            return e;
        }

        protected Entity CreateScreenlet(ScreenletComp sc)
        {
            var w = new EntityWorld();
            w.InitializeAll(true);
            Entity screenletEntity = w.CreateEntity();
            screenletEntity.AddComponent(sc);
            Screens.Add(screenletEntity);
            ActiveScreen = sc;
            ActiveWorld = w;
            return screenletEntity;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (Entity screenletEntity in Screens)
            {
                ScreenletComp screenComp = screenletEntity.GetComponent<ScreenletComp>();
                ActiveScreen = screenComp;
                ActiveWorld = screenletEntity.entityWorld;
                ActiveWorld.Update();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(ActiveScreen.BackgroundColor);

            // loop all screens/worlds and draw them.
            foreach (Entity screenletEntity in Screens)
            {
                ScreenletComp screenComp = screenletEntity.GetComponent<ScreenletComp>();
                ActiveScreen = screenComp;
                ActiveWorld = screenletEntity.entityWorld;
                screenComp.SpriteBatch.Begin();
                screenletEntity.entityWorld.Draw();
                screenComp.SpriteBatch.End();                
            }

            base.Draw(gameTime);
        }

    }
}
