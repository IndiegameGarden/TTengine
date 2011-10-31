// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TTengine;
using TTengine.Core;
using TTengine.Util;

namespace TTengineTestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        // a root gamelet that will contain all others
        Gamelet rootlet;
        Screenlet screenlet;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 640;

            // create the TTengine instance, linked to this Game class
            TTengineMaster.Create(this);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // create one root gamelet
            rootlet = new FixedTimestepPhysics();

            // init one Screenlet
            screenlet = new Screenlet(640, 480);
            screenlet.DrawColor = Color.White;
            rootlet.Add(screenlet);

            // add a FrameRateCounter utility Gamelet
            Gamelet fps = new FrameRateCounter();
            fps.DrawColor = Color.Black;
            screenlet.Add(fps);

            // add a static text 'MyTextlet'
            MyTextlet txt = new MyTextlet("TTengine shader test using Efflet");
            txt.Position = new Vector2(0.01f, 0.4f);
            txt.DrawColor = Color.Black;
            screenlet.Add(txt);

            // add several Spritelets and set some specific velocity per item
            Random rnd = new Random();
            Spritelet ball = null;
            for (float j = 0.1f; j < 1.6f; j += 0.20f)
            {
                for (float i = 0.1f; i < 1.0f; i += 0.1f)
                {
                    //Spritelet ball = new Spritelet("ball");
                    ball = new Ball();
                    ball.Position = new Vector2(j, i); //(float) rnd.NextDouble() , (float) rnd.NextDouble() );
                    ball.Velocity = 0.1f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);
                    ball.Rotate = (float)(Math.PI * 2 * rnd.NextDouble());
                    ball.Scale = 0.4f + 0.6f * (float)rnd.NextDouble();

                    ball.StartTime = 3f * (j + i);
                    ball.Duration = 4f + 5f * (float)rnd.NextDouble();

                    screenlet.Add(ball);
                }
            }

            HypnoEfflet eff = new HypnoEfflet();
            eff.Alpha = 0.2f;
            screenlet.Add(eff);


            // finally initialize our top-level (root) Gamelet using the engine
            TTengineMaster.Initialize(rootlet);

            // plus call base to enumnerate all XNA (gfx) Game components to init
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
               //
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // call the TTengine Update method on our root gamelet
            TTengineMaster.Update(gameTime, rootlet);

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // draw all my gamelet items
            TTengineMaster.Draw(gameTime, rootlet);
            base.Draw(gameTime);
        }
    }
}
