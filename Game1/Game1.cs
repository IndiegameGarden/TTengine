// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Comps;
using Artemis;

namespace Game1
{
    /// <summary>
    /// Main game class.
    /// </summary>
    public class Game1 : TTGame
    {
        public static Game1Factory Factory;
        public static new Game1 Instance;

        // the channels (i.e. layers)
        public Entity LevelChannel, BackgroundChannel, GuiChannel;

        public Entity Ship;

        public Game1()
        {
            Instance = this;
            IsAudio = false;    // set to true if audio is needed
        }

        protected override void LoadContent()
        {
            Factory = new Game1Factory();

            // create level/background channels
            BackgroundChannel = Factory.CreateChannel(Factory.New(), Color.Black);
            BackgroundChannel.C<PositionComp>().Depth = 0.7f;
            BackgroundChannel.Tag = "BackgroundChannel";

            LevelChannel = Factory.CreateChannel(Factory.New(), Color.Transparent);
            LevelChannel.C<PositionComp>().Depth = 0.5f;
            LevelChannel.Tag = "LevelChannel";
            
            // GuiChannel = TODO

            // add framerate counter
            Factory.CreateFrameRateCounter(Factory.New(), Color.White, 20 );

            // create the player ship
            Ship = Factory.CreateShip(Factory.New());

            // create the root level which contains the builder entities (with more level content)
            //Level root = new RootLevel();
         
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape))
            {
                UnloadContent();
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            BackgroundChannel.C<WorldComp>().World.UnloadContent();
            LevelChannel.C<WorldComp>().World.UnloadContent();            

            base.UnloadContent();
        }
    }

}
