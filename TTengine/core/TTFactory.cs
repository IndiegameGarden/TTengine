using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Artemis;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// The Singleton TTengine Factory to create new Entities (may be half-baked, 
    /// to further customize), and perhaps other things
    /// </summary>
    public sealed class TTFactory
    {
        private static TTGame _game = null;

        static TTFactory() {
            _game = TTGame.Instance;
        }

        /// <summary>
        /// Create simplest Entity without components
        /// </summary>
        /// <returns></returns>
        public static Entity CreateEntity()
        {
            return _game.ActiveWorld.CreateEntity();
        }

        /// <summary>
        /// Create a Gamelet, which is an Entity with position and velocity, but no shape/drawability (yet).
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
        /// Create a Drawlet, which is a moveable, drawable Entity
        /// </summary>
        /// <returns></returns>
        public static Entity CreateDrawlet()
        {
            Entity e = CreateGamelet();
            e.AddComponent(new DrawComp());
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Create a Spritelet, which is a collidable, moveable sprite 
        /// </summary>
        /// <param name="graphicsFile">The content graphics file with or without extension. If
        /// extension given eg "ball.png", the uncompiled file will be loaded at runtime. If no extension
        /// given eg "ball", precompiled XNA content will be loaded (.xnb files).</param>
        /// <returns></returns>
        public static Entity CreateSpritelet(string graphicsFile)
        {
            Entity e = CreateDrawlet();
            var spriteComp = new SpriteComp(graphicsFile);
            e.AddComponent(spriteComp);
            float radius = spriteComp.Width/2.0f;
            e.AddComponent(new ShapeComp(radius));
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Creates a Textlet, which is a moveable piece of text. (TODO: font)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Entity CreateTextlet(string text)
        {
            Entity e = CreateDrawlet();
            e.AddComponent(new ScaleComp());
            TextComp tc = new TextComp(text);
            tc.Font = _game.Content.Load<SpriteFont>("TTDebugFont"); // FIXME allow other fonts
            e.AddComponent(tc);
            return e;
        }

        /// <summary>
        /// Creates a Screenlet, which is an Entity that contains a screenComp (RenderBuffer) to 
        /// which graphics can be rendered.
        /// </summary>
        /// <returns></returns>
        public static Entity CreateScreenlet(EntityWorld world, int width, int height)
        {
            var sc = new ScreenComp(true, width, height);
            var screenlet = world.CreateEntity();
            screenlet.AddComponent(sc);
            screenlet.AddComponent(new DrawComp());
            return screenlet;
        }

    }
}

