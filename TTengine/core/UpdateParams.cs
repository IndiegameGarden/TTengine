// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /**
     * parameters collection used for Update() calls to convey global simulation knowledge.
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
        public double SimTime = 0.0;
        
        /// <summary>
        /// Delta t, the simulation time passed since last Update() (i.e. Gamelet simtime) in seconds
        /// </summary>
        public double Dt = 0.0;

        /// <summary>
        /// create all params with null or default values
        /// </summary>
        public UpdateParams()
        {
        }

        /// <summary>
        /// create params set with times according to a given GameTime
        /// </summary>
        public UpdateParams(GameTime gameTime)
        {
            CopyFrom(gameTime);
        }

        /// <summary>
        /// Copy all fields from an 'other' params to the current one. 
        /// (Useful for re-initializing avoiding new obj creation)
        /// </summary>
        public void CopyFrom(UpdateParams other)
        {
            gameTime = other.gameTime;
            SimTime = other.SimTime;
            Dt = other.Dt;
        }

        /// <summary>
        /// Fill all fields based on a XNA GameTime object
        /// </summary>
        /// <param name="gameTime"></param>
        public void CopyFrom(GameTime gameTime)
        {
            this.gameTime = gameTime;
            SimTime = (float)gameTime.TotalGameTime.TotalSeconds;
            Dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
