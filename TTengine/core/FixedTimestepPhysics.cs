// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TTengine
{
    /**
     * use at or near the root of a gametree to provide fixed-timestep calls
     * of the Update method. Useful for fixed timestep physics simulation
     * to get jitter-free sprites in XNA.
     */
    public class FixedTimestepPhysics: Gamelet
    {        
        public float simulateAheadTime = 0.050f;
        public float fixedTimestep = 0.010f;

        protected float itemsSimTime;

        /// create a Screenlet that renders to the default
        public FixedTimestepPhysics()
            : base()
        {            
        }

        protected override void OnInit()
        {
            itemsSimTime = 0f;
        }

        internal override void Update(ref UpdateParams p)
        {
            while (itemsSimTime < p.simTime + simulateAheadTime ) 
            {
                itemsSimTime += fixedTimestep;
                UpdateParams updParsCache = new UpdateParams();
                updParsCache.gameTime = p.gameTime;
                updParsCache.simTime = itemsSimTime;
                updParsCache.dt = fixedTimestep;
                base.Update(ref updParsCache);
            }
        }
       
    }
}
