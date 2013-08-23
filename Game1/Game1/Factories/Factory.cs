using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using TTengine.Core;
using TTengine.Comps;
//using Game1.Comps;

namespace Game1.Factories
{
    /// <summary>
    /// Factory to create new game-specific entities
    /// </summary>
    public class Factory: TTFactory
    {
        public static Entity CreateBall(float radius)
        {
            Entity e = CreateSpritelet("ball");
            return e;
        }
    }
}
