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
    public static class Factory
    {
        public static Entity CreateEntity()
        {
            return TTGame.Instance.World.CreateEntity();
        }

        public static Entity CreateBall(float radius)
        {
            Entity e = CreateEntity();
            e.AddComponent(new PositionComp());
            e.AddComponent(new VelocityComp());
            e.AddComponent(new ShapeComp(radius));
            return e;
        }
    }
}
