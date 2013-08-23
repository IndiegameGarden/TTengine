using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// Factory to create new Entities
    /// </summary>
    public class TTFactory
    {

        /// <summary>
        /// Create simplest Entity without components
        /// </summary>
        /// <returns></returns>
        public static Entity CreateEntity()
        {
            return TTGame.Instance.World.CreateEntity();
        }

        /// <summary>
        /// Create a Gamelet, which is an Entity with position and velocity but no shape/drawability (yet).
        /// </summary>
        /// <returns></returns>
        public static Entity CreateGamelet()
        {
            Entity e = CreateEntity();
            e.AddComponent(new PositionComp());
            e.AddComponent(new VelocityComp());
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Create a Spritelet, which is a collidable, moveable sprite in TTengine
        /// </summary>
        /// <param name="graphicsFile">The content graphics file with or without extension. If
        /// extension given eg "ball.png", the PNG file will be loaded at runtime. If no extension
        /// given eg "ball", default XNA content will be loaded (.xnb files).</param>
        /// <returns></returns>
        public static Entity CreateSpritelet(string graphicsFile)
        {
            Entity e = CreateEntity();
            e.AddComponent(new PositionComp());
            e.AddComponent(new VelocityComp());
            var spriteComp = new SpriteComp(graphicsFile);
            e.AddComponent(spriteComp);
            float radius = spriteComp.Width/2.0f;
            e.AddComponent(new ShapeComp(radius));
            e.AddComponent(new DrawComp());
            e.Refresh();
            return e;
        }

    }
}

