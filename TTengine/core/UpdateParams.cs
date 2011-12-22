// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /**
     * parameters collection used for Update() and Draw() methods of GameItems.
     * This avoids passing lists of parameters around which may change (being a refactoring
     * nightmare!). Also provides crucial rendering/updating info for items in a game-tree.
     */
    public class UpdateParams
    {
        /// <summary>
        /// GameTime class as passed by the XNA Game class Update() method
        /// </summary>
        public GameTime gameTime = null;

        /// <summary>
        /// A globally kept simulation time value in seconds, 0f is start of simulation
        /// </summary>
        public float simTime = 0.0f;
        
        /// <summary>
        /// delta t, the simulation time passed since last Update() (i.e. Gamelet simtime) in seconds
        /// </summary>
        public float dt = 0.0f;

        /**
         * create all params with null or default values
         */
        public UpdateParams()
        {
        }

        /**
         * create params set with times according to a given GameTime
         */
        public UpdateParams(GameTime gameTime)
        {
            CopyFrom(gameTime);
        }

        /// Copy all fields from an 'other' params to the current one. 
        /// (Useful for re-initializing avoiding new obj creation)
        public void CopyFrom(UpdateParams other)
        {
            gameTime = other.gameTime;
            simTime = other.simTime;
            dt = other.dt;
        }

        public void CopyFrom(GameTime gameTime)
        {
            this.gameTime = gameTime;
            simTime = (float)gameTime.TotalGameTime.TotalSeconds;
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
