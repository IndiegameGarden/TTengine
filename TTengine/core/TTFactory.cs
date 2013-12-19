using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Artemis;
using TTengine.Comps;
using TTMusicEngine.Soundevents;

namespace TTengine.Core
{
    /// <summary>
    /// The Singleton TTengine Factory to create new Entities (may be half-baked, 
    /// to further customize), and perhaps other things
    /// </summary>
    public sealed class TTFactory
    {
        /// <summary>The Artemis entity world that is currently used for building/creating new Entities in</summary>
        public static EntityWorld BuildWorld;

        /// <summary>The screen that newly built Entities by default will render to.
        /// Value null is used to denote "default".</summary>
        public static ScreenComp BuildScreen;

        private static TTGame _game = null;

        static TTFactory() {
            _game = TTGame.Instance;
        }

        public static void BuildTo(EntityWorld world)
        {
            BuildWorld = world;
        }

        public static void BuildTo(Channel channel)
        {
            BuildWorld = channel.World;
            BuildScreen = channel.Screen;
        }

        public static void BuildTo(ScreenComp screen)
        {
            BuildScreen = screen;
        }

        /// <summary>
        /// Create simplest Entity without components within the EntityWorld currently selected
        /// for Entity construction
        /// </summary>
        /// <returns></returns>
        public static Entity CreateEntity()
        {
            return BuildWorld.CreateEntity();
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
            e.AddComponent(new DrawComp(BuildScreen));
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Create a Spritelet, which is a moveable sprite 
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
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Create a Spritelet with texture based on the contents of a (child) Channel
        /// </summary>
        /// <param name="subChannel"></param>
        /// <returns></returns>
        public static Entity CreateSpritelet(Channel subChannel)
        {
            Entity e = CreateDrawlet();
            var rs = subChannel.Screen.RenderTarget;
            var spriteComp = new SpriteComp(rs);
            e.AddComponent(spriteComp);
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Create an animated sprite entity
        /// </summary>
        /// <param name="atlasBitmapFile">Filename of the sprite atlas bitmap</param>
        /// <param name="NspritesX">Number of sprites in horizontal direction (X) in the atlas</param>
        /// <param name="NspritesY">Number of sprites in vertical direction (Y) in the atlas</param>
        /// <returns></returns>
        public static Entity CreateAnimatedSpritelet(string atlasBitmapFile, int NspritesX, int NspritesY)
        {
            return CreateAnimatedSpritelet(atlasBitmapFile, NspritesX, NspritesY, AnimationType.NORMAL);
        }

            /// <summary>
        /// Create an animated sprite entity
        /// </summary>
        /// <param name="atlasBitmapFile">Filename of the sprite atlas bitmap</param>
        /// <param name="NspritesX">Number of sprites in horizontal direction (X) in the atlas</param>
        /// <param name="NspritesY">Number of sprites in vertical direction (Y) in the atlas</param>
        /// <param name="animType">Animation type chosen from AnimationType class</param>
        /// <returns></returns>
        public static Entity CreateAnimatedSpritelet(string atlasBitmapFile, int NspritesX, int NspritesY, AnimationType animType)
        {
            Entity e = CreateDrawlet();
            var spriteComp = new AnimatedSpriteComp(atlasBitmapFile,NspritesX,NspritesY);
            spriteComp.AnimType = animType;
            e.AddComponent(spriteComp);
            e.Refresh();
            return e;
        }

        public static Entity CreateSpriteField(string fieldBitmapFile, string spriteBitmapFile)
        {
            Entity e = CreateDrawlet();
            var spriteFieldComp = new SpriteFieldComp(fieldBitmapFile);
            var spriteComp = new SpriteComp(spriteBitmapFile);
            spriteFieldComp.FieldSpacing = new Vector2(spriteComp.Width, spriteComp.Height);
            e.AddComponent(spriteComp);
            e.AddComponent(spriteFieldComp);
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
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Creates a Screenlet that renders to default viewport
        /// </summary>
        /// <returns></returns>
        public static Entity CreateScreenlet()
        {
            var sc = new ScreenComp();
            var e = CreateEntity();
            e.AddComponent(sc);
            e.AddComponent(new DrawComp(BuildScreen));
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Creates a Screenlet that may contain a screenComp (RenderBuffer) to 
        /// which graphics can be rendered. 
        /// </summary>
        /// <returns></returns>
        public static Entity CreateScreenlet(int width, int height)
        {
            var sc = new ScreenComp(width, height);
            var e = CreateEntity();
            e.AddComponent(sc);
            e.AddComponent(new DrawComp(BuildScreen));
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Creates a Channel that is a child of the current SelectedChannel, that
        /// renders to a RenderTarget of specified width/height
        /// </summary>
        /// <returns>Created channel.</returns>
        public static Channel CreateChannel(int width, int height)
        {
            var ch = new Channel(width,height);
            ch.Register();
            return ch;
        }

        /// <summary>
        /// Creates a root Channel that renders to the default backbuffer, having
        /// a specified background Color.
        /// </summary>
        /// <param name="backgroundColor">The default background Color for the Channel</param>
        /// <returns>Created channel.</returns>
        public static Channel CreateChannel(Color backgroundColor)
        {
            var ch = new Channel();
            ch.Screen.BackgroundColor = backgroundColor;
            ch.Register();
            return ch;
        }

        /// <summary>
        /// Creates a Scriptlet, which is an Entity that only contains a custom code script
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static Entity CreateScriptlet(IScript script)
        {
            var e = CreateEntity();
            e.AddComponent(new ScriptComp(script));
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Create an Audiolet, which is an Entity that only contains an audio script
        /// </summary>
        /// <param name="soundScript"></param>
        /// <returns></returns>
        public static Entity CreateAudiolet(SoundEvent soundScript)
        {
            var e = CreateEntity();
            e.AddComponent(new AudioComp(soundScript));
            e.Refresh();
            return e;
        }
    }
}
