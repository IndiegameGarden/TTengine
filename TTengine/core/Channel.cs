using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// A convenience wrapper around an EntityWorld and a Screenlet to which the world renders.
    /// </summary>
    public class Channel
    {
        public bool IsActive = false;
        public ScreenletComp Screen;
        public EntityWorld World;

        internal Channel(TTGame game)
        {
            World = new EntityWorld();
            World.InitializeAll(true);
            // TODO other screen sizes
            Screen = new ScreenletComp(true, game.GraphicsMgr.PreferredBackBufferWidth, game.GraphicsMgr.PreferredBackBufferHeight);
            Entity screenletEntity = World.CreateEntity();
            screenletEntity.AddComponent(Screen);
            screenletEntity.AddComponent(new DrawComp());
        }

    }
}
