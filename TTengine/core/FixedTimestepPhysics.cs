// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

namespace TTengine.Core
{
    /**
     * use at or near the root of a gametree to provide fixed-timestep calls
     * of the Update method. Useful for fixed timestep physics simulation
     * to get jitter-free sprites in XNA.
     */
    public class FixedTimestepPhysics: Gamelet
    {        
        public float simulateAheadTime = 0.050f; // TODO adjustable?
        public float fixedTimestep = 0.010f; // TODO adjustable?

        protected float itemsSimTime = 0f;

        internal void Update(ref UpdateParams p)
        {
            while (itemsSimTime < p.SimTime + simulateAheadTime ) 
            {
                itemsSimTime += fixedTimestep;
                UpdateParams updParsCache = new UpdateParams();
                updParsCache.gameTime = p.gameTime;
                updParsCache.SimTime = itemsSimTime;
                updParsCache.Dt = fixedTimestep;
                //base.Update(ref updParsCache);
            }
        }
       
    }
}
