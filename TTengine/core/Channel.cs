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
        public Entity Screen;
        public EntityWorld World;

        internal Channel(TTGame game)
        {
            World = new EntityWorld();
            World.InitializeAll(true);
            // TODO other screen sizes - move creation to TTfactory
            var ScreenComp = new ScreenComp(true, game.GraphicsMgr.PreferredBackBufferWidth, game.GraphicsMgr.PreferredBackBufferHeight);
            Screen = World.CreateEntity();
            Screen.AddComponent(ScreenComp);
            Screen.AddComponent(new DrawComp());
        }

    }
}
