﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// A convenience wrapper around an EntityWorld and a Screenlet to which the world renders.
    /// <seealso cref="ChannelManager"/>
    /// </summary>
    public class Channel
    {
        /// <summary>If true, the World of this channel is actively simulated (Updated)</summary>
        public bool IsActive = false;

        /// <summary>If true, the World of this channel is actively rendered (Drawn) to the Screenlet</summary>
        public bool IsVisible = false;

        /// <summary></summary>
        public Entity Screenlet;

        /// <summary></summary>
        public EntityWorld World;

        internal Channel(TTGame game)
        {
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
            // TODO other screenComp sizes?
            TTFactory.BuildWorld = this.World;
            this.Screenlet = TTFactory.CreateScreenlet(); 
        }

    }
}
