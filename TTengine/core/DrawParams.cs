// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /**
     * parameters collection used for Draw() method of GameItems. This avoids a refactoring
     * nightmare if in the future more parameters are passed.
     */
    public class DrawParams
    {
        /// GameTime as passed by the XNA Game class
        public GameTime gameTime = null;

        /// create all params with null values
        public DrawParams()
        {
        }

        /// create params set with times according to a given GameTime
        public DrawParams(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

    }
}
