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
        /// The time factor for this World; 1.0 is normal, < 1.0 is slower time and > 1.0 is faster time.
        /// </summary>
        public double TimeWarp = 1.0;

		/// <summary>
		/// The Screen that this World renders to, or null if not rendering to a specific Screen.
		/// </summary>
		public ScreenComp Screen = null;

        public WorldComp()
        {
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
        }
    }
}
