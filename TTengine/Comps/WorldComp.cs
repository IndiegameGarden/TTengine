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

        public WorldComp()
        {
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
        }
    }
}
