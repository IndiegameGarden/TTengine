using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Artemis;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;
using PXengine.Comps;

namespace PXengine.Core
{
    /// <summary>
    /// base class for all Pixie levels (common functions)
    /// </summary>
    public abstract class PXLevel: IScript
    {
        public PXLevel()
        {
            Subtitles = new SubtitleManager();
        }

        public string LevelBitmapFile = "";

        // some default colors and settings that may be changed by Level subclasses
        public static Color PIXIE_COLOR = new Color(251, 101, 159); // pink
        public float  DEFAULT_SCALE = 1f;
        public double  SCREEN_SCROLLING_SPEED = 10.0;
        public double PIXIE_TARGETSPEED = 10.0;
        public double PIXIE_SPEED = 10.0;
        public int DefaultPassableIntensityThreshold = 380;
        public Vector2 PIXIE_STARTING_POS = Vector2.Zero; // in pixels        
        public Vector2 BG_STARTING_POS = Vector2.Zero;    // in pixels; bg=background

        /// <summary>
        /// scrolling screen trigger boundaries (in pixels)
        /// </summary>
        public bool isBackgroundScrollingOn = true;
        public float BOUND_X = 30f;
        public float BOUND_Y = 30f;

        /// <summary>
        /// default color of the background (e.g. for areas not covered by the bg bitmap)
        /// </summary>
        public Color BackgroundColor = Color.White;

        /// <summary>
        /// The factory for creating new entities in the game
        /// </summary>
        public GameFactory Factory = GameFactory.Instance;

        /// <summary>
        /// level music
        /// </summary> 
        public GameMusic Music;

        /// <summary>
        /// background bitmap
        /// </summary>
        public Entity Background;

        /// <summary>
        /// our heroine Pixie
        /// </summary>
        public Entity Pixie;

        public SubtitleManager Subtitles;

        protected double timeEscDown = 0.0;

        /// <summary>
        /// Init: the scrolling level itself.
        /// </summary>
        protected virtual void InitLevel()
        {
            Background = Factory.CreateLevelet(LevelBitmapFile);
            var sc = new ScrollingComp(this.PIXIE_STARTING_POS);
            Pixie.AddComponent(sc);
            sc.Scrolling.Current = this.BG_STARTING_POS;
            sc.Scrolling.Speed = this.SCREEN_SCROLLING_SPEED;

            //Motion.Scale = DEFAULT_SCALE;
            //Motion.ScaleTarget = DEFAULT_SCALE;
            //MySpriteBatch = new TTSpriteBatch(Screen.graphicsDevice, SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            TTFactory.BuildScreen.Zoom = this.DEFAULT_SCALE;

            // inits based on level
            TTFactory.BuildScreen.BackgroundColor = this.BackgroundColor;

        }

        /// <summary>
        /// Init: pixie herself (a default implementation is in PixieLevel class)
        /// </summary>
        protected virtual void InitPixie()
        {
            Pixie = Factory.CreatePixie(PIXIE_COLOR);
            Pixie.GetComponent<PositionComp>().Position = PIXIE_STARTING_POS;
            Pixie.GetComponent<TargetMotionComp>().Target = PIXIE_STARTING_POS;
            Pixie.GetComponent<TargetMotionComp>().TargetVelocity = PIXIE_TARGETSPEED;
            Pixie.GetComponent<ControlComp>().TimeBetweenMoves = (1 / PIXIE_SPEED);
        }

        /// <summary>
        /// Init: bad pixels (enemies)
        /// </summary>
        protected virtual void InitBadPixels()
        {
        }

        /// <summary>
        /// Init: toys (=weapons)
        /// </summary>
        protected virtual void InitToys()
        {
        }

        /// <summary>
        /// Init: level-specific items (not fitting in the existing init categories) to be initialized by subclasses
        /// </summary>
        protected abstract void InitLevelSpecific();

        public void Init()
        {
            InitPixie();
            InitLevel();
            InitBadPixels();
            InitToys();
            InitLevelSpecific();
        }

        /// check keys specific for level
        protected virtual void LevelKeyControl(ScriptContext ctx)
        {
            KeyboardState st = Keyboard.GetState();
            if (st.IsKeyDown(Keys.Escape))
            {
                timeEscDown += ctx.Dt;
                //MotionB.ScaleTarget = 1.5f*DEFAULT_SCALE;
                //MotionB.ScaleSpeed = 0.0004f;
                //Motion.RotateModifier = timeEscDown * 0.05f;                
            }
            else
            {
                timeEscDown = 0f;
                //MotionB.ScaleTarget = DEFAULT_SCALE; // TODO
            }
            if (timeEscDown > 1.0f)
            {
                PXGame.Instance.Exit();
            }

            /*
            // FIXME remove debug keys
            if (st.IsKeyDown(Keys.PageUp) && Motion.Zoom == Motion.ZoomTarget)
            {
                Motion.ZoomTarget *= 2.0f;
                Motion.ZoomSpeed = 0.02f;
            }

            if (st.IsKeyDown(Keys.PageDown) && Motion.Zoom == Motion.ZoomTarget)
            {
                Motion.ZoomTarget *= 0.5f;
                Motion.ZoomSpeed = 0.02f;
            }

            if (st.IsKeyDown(Keys.RightControl))
                pixie.IsCollisionFree = true;
            else
                pixie.IsCollisionFree = false;

             */
        }

        // scroll the level background to match pixie
        /*
        protected virtual void ScrollBackground(ref UpdateParams p)
        {
            // scrolling background at borders
            Vector2 pixiePos = pixie.Motion.PositionAbs;

            if (pixiePos.X < BOUND_X || pixiePos.X > (Screen.Width - BOUND_X) ||
                pixiePos.Y < BOUND_Y || pixiePos.Y > (Screen.Height - BOUND_Y))
            {
                if (ScreenBorderHit())
                    Background.Target = pixie.Position;
            }
        }
        */

        /// <summary>
        /// can be overridden with custom functions if screen border is hit by pixie
        /// </summary>
        protected virtual bool ScreenBorderHit()
        {
            return true;
        }

        public void OnUpdate(ScriptContext ctx)
        {
            LevelKeyControl(ctx);
            /*
            base.OnUpdate(ref p);

            // important: reflect the global viewpos (for sprites to use)
            Thing.ViewPos = Background.Position;

            // do some level tasks            
            if (isBackgroundScrollingOn)
                ScrollBackground(ref p);
            */
        }

        public void OnDraw(ScriptContext ctx)
        {
        }
    }
}
