// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /**
     * the Master is a top-level entry point to TTengine
     */
    public class TTengineMaster
    {
        public static Game ActiveGame = null;
        //public static GraphicsDeviceManager ActiveGraphicsDeviceManager = null;

        public static void Create(Game game)
        {
           ActiveGame = game;           
        }

        /// Initialize a root Gamelet
        public static void Initialize(Gamelet rootGamelet)
        {
            rootGamelet.Initialize();            
        }

        public static void Update(GameTime gameTime, Gamelet rootGamelet)
        {
            UpdateParams up = new UpdateParams(gameTime);
            rootGamelet.Update(ref up);
        }

        public static void Draw(GameTime gameTime, Gamelet rootGamelet)
        {
            DrawParams dp = new DrawParams(gameTime);
            rootGamelet.Draw(ref dp);
        }
    }
}
