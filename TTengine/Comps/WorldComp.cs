using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine.Core;
using Artemis;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Component that contains a (separate) EntityWorld
    /// </summary>
    public class WorldComp: IComponent
    {
        /// <summary>The EntityWorld that is being contained in this component</summary>
        public EntityWorld World;

        /// <summary>
        /// The time factor for this World; 1.0 is normal, < 1 is slower time and > 1 is faster time.
        /// </summary>
        public double TimeWarp = 1.0;

        public WorldComp()
        {
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
        }
    }
}
