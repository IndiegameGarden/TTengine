// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Artemis;
using TTengine.Comps;
using TTengine.Modifiers;
using TTMusicEngine.Soundevents;
using TTengine.Factories.Shape3DFactoryItems;

namespace TTengine.Core
{
    /// <summary>
    /// The TTengine's Factory to create, transform or customize Entities 
    /// with selection methods to which EntityWorld or Screen the items are built.
    /// 
    /// Factory's methods are in the following classes:
    ///     New....()           for new Entity creation from nothing.
    ///     Create....(e,input) for creating from Entity e a usable stereotyped Entity (e.g., a Sprite)
    ///     Add....(e, input)   for adding elements such as a Component or Script to an Entity
    ///     BuildTo(...)        for changing the build destination of the factory
    ///     Process...(e, par)  for processing an entity, e.g. transforming it, configuring or modifying.
    ///     Join...(e1, e2 ..)  for joining Entities e1/e2/.. together in some way, composing/arranging.
    ///     
    /// </summary>
    public class TTFactory
    {
        protected EntityWorld buildWorld;

        protected ScreenComp buildScreen;

        public TTFactory()
        {
            this.buildWorld = TTGame.Instance.RootWorld;
            this.buildScreen = TTGame.Instance.RootScreen;
        }

        public TTFactory(TTFactory parent)
        {
            this.buildWorld = parent.buildWorld;
            this.buildScreen = parent.buildScreen;
        }

        internal void BuildTo(EntityWorld world)
        {
            buildWorld = world;
        }

        internal void BuildTo(ScreenComp screen)
        {
            buildScreen = screen;
        }

        /// <summary>The Artemis entity world currently used by this Factory for building new Entities in.</summary>
        public EntityWorld BuildWorld { get { return buildWorld;  } }

        /// <summary>The Screen that newly built Entities in this Factory will render to.</summary>
        public ScreenComp BuildScreen {  get { return buildScreen;  } }

        /// <summary>
        /// Switch factory's building output to the root channel of the game.
        /// </summary>
        /// <returns></returns>
        public FactoryState BuildToRoot()
        {
            var st = new FactoryState(this, buildWorld, buildScreen);
            BuildTo(TTGame.Instance.RootWorld);
            BuildTo(TTGame.Instance.RootScreen);
            return st;
        }

        /// <summary>
        /// Switch factory's building output to given Channel, World, Layer, or Screen
        /// </summary>
        /// <param name="e">an Entity which may contain a WorldComp, a ScreenComp, or both. In case of both,
        /// the Entity is a Channel.</param>
        public FactoryState BuildTo(Entity e)
        {
            var st = new FactoryState(this, buildWorld, buildScreen);
            if (e.HasC<WorldComp>())
            {
                var wc = e.C<WorldComp>();
                buildWorld = wc.World;
                if (wc.Screen != null)
                    buildScreen = wc.Screen;
            }
            if (e.HasC<ScreenComp>())
                buildScreen = e.C<ScreenComp>();
            return st;
        }

        /// <summary>
        /// Switch factory's building target to the same as the given other factory f
        /// </summary>
        /// <param name="f"></param>
        public FactoryState BuildLike(TTFactory f)
        {
            var st = new FactoryState(this, buildWorld, buildScreen);
            BuildTo(f.buildWorld);
            BuildTo(f.buildScreen);
            return st;
        }

        /// <summary>
        /// Create simplest Entity without components within the EntityWorld currently selected
        /// for Entity construction.
        /// </summary>
        /// <returns>New empty Entity which is enabled</returns>
        public Entity New()
        {
            Entity e = buildWorld.CreateEntity();
            return e;
        }

        /// <summary>
        /// Create simplest Entity without components within the EntityWorld currently selected
        /// for Entity construction. By default, the Entity is not enabled until it is
        /// Finalized.
        /// </summary>
        /// <returns>New empty Entity which is disabled</returns>
        public Entity NewDisabled()
        {
            Entity e = buildWorld.CreateEntity();
            e.IsEnabled = false;
            return e;
        }

        /// <summary>
        /// Finalize Entity after having constructed all its components; enabling it and
        /// triggering a refresh.
        /// </summary>
        /// <param name="e">Entity to finalize and in the next ECS round activate.</param>
        public virtual void Finalize(Entity e)
        {
            e.IsEnabled = true;
            e.Refresh();
        }

        /// <summary>
        /// Create a Sprite, which is a moveable graphic
        /// </summary>
        /// <param name="graphicsFile">The content graphics file with or without extension. If
        /// extension given eg "ball.png", the uncompiled file will be loaded at runtime. If no extension
        /// given eg "ball", precompiled XNA content will be loaded (.xnb files).</param>
        public Entity CreateSprite(Entity e, string graphicsFile)
        {
            AddDrawable(e);
            var sc = new SpriteComp(graphicsFile);
            e.AddC(sc);
            return e;
        }

        /// <summary>
        /// Create a Sprite with texture based on the contents of a Screen
        /// </summary>
        /// <returns></returns>
        public Entity CreateSprite(Entity e, ScreenComp screen)
        {
            AddDrawable(e);
            var sc = new SpriteComp(screen);
            e.AddC(sc);
            return e;
        }

        /// <summary>
        /// Create an animated sprite
        /// </summary>
        /// <param name="atlasBitmapFile">Filename of the sprite atlas bitmap</param>
        /// <param name="NspritesX">Number of sprites in horizontal direction (X) in the atlas</param>
        /// <param name="NspritesY">Number of sprites in vertical direction (Y) in the atlas</param>
        /// <param name="animType">Animation type chosen from AnimationType class</param>
        /// <param name="slowDownFactor">Slowdown factor for animation, default = 1</param>
        public Entity CreateAnimatedSprite(Entity e, string atlasBitmapFile, int NspritesX, int NspritesY, 
                                                     AnimationType animType = AnimationType.NORMAL,
                                                     int slowDownFactor = 1)
        {
            AddDrawable(e);
            var sc = new AnimatedSpriteComp(atlasBitmapFile,NspritesX,NspritesY);
            sc.AnimType = animType;
            sc.SlowdownFactor = slowDownFactor;
            e.AddC(sc);
            return e;
        }

        /// <summary>
        /// experimental TODO
        /// </summary>
        /// <param name="e"></param>
        /// <param name="fieldBitmapFile"></param>
        /// <param name="spriteBitmapFile"></param>
        /// <returns></returns>
        public Entity CreateSpriteField(Entity e, string fieldBitmapFile, string spriteBitmapFile)
        {
            AddDrawable(e);
            var sfc = new SpriteFieldComp(fieldBitmapFile);
            var sc = new SpriteComp(spriteBitmapFile);
            sfc.FieldSpacing = new Vector2(sc.Width, sc.Height);
            e.AddC(sc);
            e.AddC(sfc);
            return e;
        }

        /// <summary>
        /// Creates a moveable text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        public Entity CreateText(Entity e, string text, string fontName = "TTDefaultFont")
        {
            AddDrawable(e);
            if (!e.HasC<ScaleComp>())  e.AddC(new ScaleComp());
            var tc = new TextComp(text, fontName);
            e.AddC(tc);
            return e;
        }

        /// <summary>
        /// Creates a Screen, an Entity that has a ScreenComp to 
        /// which graphics can be rendered. 
        /// </summary>
        /// <param name="backgroundColor">Background color of the Screen</param>
        /// <param name="hasRenderBuffer">if true, Screen will have its own render buffer</param>
        /// <param name="height">Screen height, if not given uses default backbuffer height</param>
        /// <param name="width">Screen width, if not given uses default backbuffer width</param>
        /// <param name="isSprite">If set to true, will also make this Entity a Sprite, with bitmap being the screen's contents.</param>
        /// <returns>Newly created Entity with a ScreenComp and DrawComp.</returns>
        public Entity CreateScreen(Entity e, Color backgroundColor, bool hasRenderBuffer = false,
                                        int width = 0, int height = 0, bool isSprite = false)
        {
            AddDrawable(e);
            var sc = new ScreenComp(hasRenderBuffer, width, height);
            sc.BackgroundColor = backgroundColor;            
            e.AddC(sc);
            if (isSprite)
            {
                e.AddC(new SpriteComp(sc));
            }
            return e;
        }

        /// <summary>
        /// Creates a Screen Sprite, which has a ScreenComp to which graphics can be rendered and also is plotted as 
        /// a sprite with the texture being the Screen's contents. Parameters as for CreateScreen().
        /// </summary>
        public Entity CreateScreenSprite(Entity e, Color backgroundColor, bool hasRenderBuffer = false,
                                        int width = 0, int height = 0)
        {
            return CreateScreen(e, backgroundColor, hasRenderBuffer, width, height, true);
        }

        /// <summary>
        /// Create a shader effect layer to which one or more Entities/sprites can render
        /// </summary>
        /// <param name="e">Entity that will carry the fx layer</param>
        /// <param name="fxFile">name of shader effect in Content (name).fx </param>
        /// <returns></returns>
        public Entity CreateFx(Entity e, string fxFile)
        {
            var fx = TTGame.Instance.Content.Load<Effect>(fxFile);
            if (!e.HasC<ScreenComp>()) e.AddC(new ScreenComp(buildScreen.RenderTarget));
            e.C<ScreenComp>().SpriteBatch.effect = fx; // set the effect in SprBatch
            return e;
        }

        /// <summary>
        /// Creates an FX Sprite that renders a shader Effect in a rectangle. By default, the rect is
        /// of screen-filling size (when width=height=0).
        /// </summary>
        /// <param name="width">width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <returns></returns>
        public Entity CreateFxSprite(Entity e, string fxFile, int width = 0, int height = 0)
        {
            AddDrawable(e);
            var fx = TTGame.Instance.Content.Load<Effect>(fxFile);
            var sc = new ScreenComp(buildScreen.RenderTarget); // renders to the existing screen buffer
            sc.SpriteBatch.effect = fx; // set the effect in SprBatch
            e.AddC(sc);
            var spc = new SpriteRectComp { Width = width, Height = height };
            e.AddC(spc);
            e.C<DrawComp>().DrawScreen = sc; // let sprite draw itself to the effect-generating Screen sc
            return e;
        }

        /// <summary>
        /// Creates a new FrameRateCounter. TODO: screen position set.
        /// </summary>
        /// <returns></returns>
        public Entity CreateFrameRateCounter(Entity e, Color textColor, int pixelsOffsetVertical = 0)
        {
            CreateText(e, "##", "TTFrameRateCounter");
            e.C<PositionComp>().Position = new Vector3(2f, 2f + pixelsOffsetVertical,0f);
            e.C<DrawComp>().DrawColor = textColor;
            AddScript(e, new Util.FrameRateCounter(e.C<TextComp>()));
            return e;
        }

        public Entity CreateSphere(Entity e, Vector3 pos, float diameter = 1.0f, int tesselation = 16)
        {
            AddDrawable(e);
            e.C<PositionComp>().Position = pos;
            var gc = new GeomComp(new SpherePrimitive(BuildScreen.SpriteBatch.GraphicsDevice , diameter, tesselation));
            e.AddC(gc);
            return e;
        }

        /// <summary>
        /// Creates a new Channel, which is an Entity with inside it a separate EntityWorld containing a dedicated ScreenComp to
        /// which that World renders, which can be then shown as a sprite. Parameters are same as for CreateScreen() above.
		/// In summary: A World inside a Sprite.
        /// </summary>
        /// <param name="backgroundColor">Background drawing color</param>
        /// <param name="width">Channel's screen width or if not given or 0 will use RenderBuffer width</param>
        /// <param name="height">Channel's screen height or if not given or 0 will use RenderBuffer height</param>
        /// <returns></returns>
        public Entity CreateChannel(Entity eChan, Color backgroundColor,
                                        int width = 0, int height = 0)
        {
			var wc = new WorldComp(); // create world

            // create a Screen Entity within the Channel's (sub) world
            Entity eScr = wc.World.CreateEntity();  // Note: creation should happen within wc.World ! See notebook 17-dec-2017
			var sc = new ScreenComp (true, width, height);
			wc.Screen = sc;				// store the Screen as part of the World
			sc.BackgroundColor = backgroundColor;
			eScr.AddC (sc);
            // add the new World
            eChan.AddC(wc);

            // create the Sprite
            CreateSprite(eChan, sc);

            return eChan;
        }

        /// <summary>
        /// Create a new channel with same properties as an existing (template) channel. Color and size will
        /// be the same.
        /// </summary>
        /// <param name="templateChannel">Existing channel to use as template for color and size.</param>
        public Entity CreateChannelLike(Entity e, Entity templateChannel) 
        {
            ScreenComp sc = templateChannel.C<WorldComp>().Screen;
            return CreateChannel(e,sc.BackgroundColor, sc.Width, sc.Height);
        }

        /// <summary>
        /// Create a new channel with CRT shader effect
        /// </summary>
        /// <param name="chan"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Entity CreateCrtChannel(Entity chan, Color backgroundColor, int width, int height)
        {
            var fx = CreateFx(New(), "crt-lottes");

            using (BuildTo(fx))
            {
                CreateChannel(chan, backgroundColor, width, height);
                ProcessFitSize(chan, BuildScreen);
            }

            EffectParameterCollection p = fx.C<ScreenComp>().SpriteBatch.effect.Parameters;
            p["video_size"].SetValue(new Vector2(chan.C<SpriteComp>().Width, chan.C<SpriteComp>().Height));
            p["output_size"].SetValue(new Vector2(BuildScreen.Width, BuildScreen.Height));
            p["texture_size"].SetValue(new Vector2(chan.C<SpriteComp>().Width, chan.C<SpriteComp>().Height));
            //p["modelViewProj"].SetValue(Matrix.Identity);

            return chan;
        }

        /// <summary>
        /// Add Entity position and velocity
        /// </summary>
        public Entity AddMotion(Entity e)
        {
            if (!e.HasC<PositionComp>()) e.AddC(new PositionComp());
            if (!e.HasC<VelocityComp>()) e.AddC(new VelocityComp());
            return e;
        }

        /// <summary>
        /// Add Entity position and velocity, and make it drawable
        /// </summary>
        public Entity AddDrawable(Entity e)
        {
            AddMotion(e);
            if (!e.HasC<DrawComp>()) e.AddC(new DrawComp(buildScreen));
            return e;
        }

        /// <summary>
        /// Add Entity scalability
        /// </summary>
        public Entity AddScalable(Entity e, double initialScale = 1.0)
        {
            if (!e.HasC<ScaleComp>()) e.AddC(new ScaleComp(initialScale));
            return e;
        }

        /// <summary>
        /// Add audio script to Entity
        /// </summary>
        public Entity AddAudio(Entity e, SoundEvent soundScript)
        {
            if (e.HasC<AudioComp>())
                e.C<AudioComp>().AudioScript = soundScript;
            else
                e.AddC(new AudioComp(soundScript));
            return e;
        }

        /// <summary>
        /// Add custom code script to Entity
        /// </summary>
        public Entity AddScript(Entity e, IScript script)
        {
            if (!e.HasC<ScriptComp>())
                e.AddC(new ScriptComp(e));
            var sc = e.C<ScriptComp>();
            sc.Scripts.Add(script);
            return e;
        }

        /// <summary>
        /// Add a script to an Entity, based on a function (delegate)
        /// </summary>
        /// <param name="e">The Entity to add script to</param>
        /// <param name="scriptCode">Script to run, as delegate code</param>
        public Entity AddScript(Entity e, ScriptDelegate scriptCode)
        {
            return AddScript(e,new BasicScript(scriptCode));
        }

        /// <summary>
        /// Add a Function script to an Entity, based on a code block (delegate) and a Function
        /// </summary>
        /// <param name="e">Entity to add function script to</param>
        /// <param name="scriptCode">Code block (delegate) that is the script with call parameters (ScriptComp,double)</param>
        /// <param name="func">Function whose value will be passed to script code each call</param>
        /// <returns></returns>
        public Entity AddFunctionScript(Entity e, FunctionScriptDelegate scriptCode, IFunction func)
        {
            return AddScript(e,new FunctionScript(scriptCode, func));            
        }

        /// <summary>
        /// Basic script object that can run code from a Delegate in the OnUpdate cycle
        /// </summary>
        public class BasicScript : IScript
        {
            protected ScriptDelegate scriptCode;

            public BasicScript(ScriptDelegate scriptCode)
            {
                this.scriptCode = scriptCode;
            }

            public void OnUpdate(ScriptComp sc)
            {
                scriptCode(sc);
            }

            public void OnDraw(ScriptComp sc) {; }
        }

        /// <summary>
        /// Apply a channel/screen/sprite fit (with scale, move) such that the entToFit will be centered in
        /// and shrunk or stretched to the extent that it optimally fits the screen parentScr.
        /// </summary>
        /// <param name="entToFit">Screen/Channel/Sprite to fit to parentScr</param>
        /// <param name="parentScr">Parent Screen</param>
        public Entity ProcessFitSize(Entity entToFit, ScreenComp parentScr, bool canStretch = true,
                                             bool canShrink = true, int maxPixelsCutOffVertical = 0)
        {
            int h, w; // height / width of entToFit

            if (entToFit.HasC<WorldComp>())
            {
                h = entToFit.C<WorldComp>().Screen.Height;
                w = entToFit.C<WorldComp>().Screen.Width;
            }
            else if (entToFit.HasC<ScreenComp>())
            {
                h = entToFit.C<ScreenComp>().Height;
                w = entToFit.C<ScreenComp>().Width;
            }
            else if (entToFit.HasC<SpriteComp>())
            {
                h = entToFit.C<SpriteComp>().Height;
                w = entToFit.C<SpriteComp>().Width;
            }
            else
                throw new NotImplementedException("Unrecognized entToFit in ProcessFitSize()");

            PositionComp pos = entToFit.C<PositionComp>();
            SpriteComp spr = entToFit.C<SpriteComp>();
            ScaleComp scl = entToFit.C<ScaleComp>();

            float scale = 1.0f;

            // if no scale comp yet, add one
            if (scl == null)
            {
                scl = new ScaleComp();
                entToFit.AddC(scl);
            }

            // position channel to the middle of parent.
            pos.PositionXY = parentScr.Center;
            spr.CenterToMiddle();

            // squeeze in to fit width
            if (canShrink && w > parentScr.Width)
            {
                scale = ((float)parentScr.Width) / ((float)w);
                // squeeze in to fit height
                if ((((h - maxPixelsCutOffVertical) * scale)) > (parentScr.Height * scale))
                {
                    scale *= ((float)parentScr.Height) / ((float)(h - maxPixelsCutOffVertical));
                }
            }

            // expand to fit width
            if (canStretch && w < parentScr.Width)
            {
                scale = ((float)parentScr.Width) / ((float)w);
                // squeeze in to fit height
                if ((scale * (float)h - (float)maxPixelsCutOffVertical) > parentScr.Height)
                {
                    scale *= ((float)parentScr.Height) / ((float)(h - maxPixelsCutOffVertical));
                }
            }

            // apply scale
            scl.Scale = scale;

            return entToFit;
        }



        /// <summary>
        /// Apply a channel/screen/sprite fit (with scale, move) such that the entToFit will be centered in
        /// and shrunk or stretched to the extent that it optimally fits the parentEnt.
        /// </summary>
        /// <param name="entToFit">Screen/Channel/Sprite to fit to parentEnt</param>
        /// <param name="parentEnt">Parent Screen or Channel</param>
        public Entity ProcessFitSize(Entity entToFit, Entity parentEnt, bool canStretch = true, 
                                                bool canShrink = true, int maxPixelsCutOffVertical = 0)
        {
            ScreenComp parentScr;
            if (parentEnt.HasC<WorldComp>())
                parentScr = parentEnt.C<WorldComp>().Screen;
            else if (parentEnt.HasC<ScreenComp>())
                parentScr = parentEnt.C<ScreenComp>();
            else
                throw new NotImplementedException("Unrecognized parentEnt in ProcessFitSize()");

            return ProcessFitSize(entToFit, parentScr, canStretch, canShrink, maxPixelsCutOffVertical);
        }

        /// <summary>
        /// Get the shader Fx parameters for the Effect that is used to render the Entity e
        /// </summary>
        /// <param name="e">An Entity with a DrawComp</param>
        /// <returns>ScreenComp's spritebatch's effect's parameters collection, an EffectParameterCollection.
        /// Or null in case the Entity renders without Effect to the screen.</returns>
        public EffectParameterCollection GetFxParameters(Entity e)
        {
            if (!e.HasC<DrawComp>())
                return null;
            if (e.C<DrawComp>().DrawScreen == null)
                return null;
            if (e.C<DrawComp>().DrawScreen.SpriteBatch.effect == null)
                return null;

            return e.C<DrawComp>().DrawScreen.SpriteBatch.effect.Parameters;
        }


    }
}
