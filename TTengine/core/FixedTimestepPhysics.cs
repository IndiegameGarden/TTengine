// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
using System;

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

        /// <summary>
        /// Time it took for last Update() of this gamelet
        /// </summary>
        public long LastUpdateDurationMs = 0;

        public FixedTimestepPhysics():base()
        {
        }

        internal override void Update(ref UpdateParams p)
        {
            long t1 = DateTime.Now.Ticks;
            while (itemsSimTime < p.SimTime + simulateAheadTime ) 
            {
                itemsSimTime += fixedTimestep;
                UpdateParams updParsCache = new UpdateParams();
                updParsCache.gameTime = p.gameTime;
                updParsCache.SimTime = itemsSimTime;
                updParsCache.Dt = fixedTimestep;
                base.Update(ref updParsCache);
            }
            long t2 = DateTime.Now.Ticks;
            LastUpdateDurationMs = (t2 - t1) / TimeSpan.TicksPerMillisecond;
        }
       
    }
}
