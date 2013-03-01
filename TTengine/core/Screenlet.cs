// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TTengine.Core
{
    /**
     * a Gamelet that contains a screen buffer to which child Gamelets will render graphics.
     *
     */
    public class Screenlet : Gamelet
    {
        /// ref to the GraphicsDevice to use by child Gamelets
        public GraphicsDevice graphicsDevice;

        /// list of all Spritelets for which mutual collision detection is done
        public List<Gamelet> collisionObjects;

        /// <summary>
        /// The center coordinate of the screen
        /// </summary>
        public Vector2 Center = Vector2.Zero;

        /**
         * The MotionComplet class specifically adapted for use by Screenlet
         */
        class ScreenletMotion : MotionComplet
        {
            public override Vector2 PositionAbs
            {
                get { return Position + PositionModifier; }
            }

            public override Vector2 PositionAbsZoomed
            {
                get { return PositionAbs; } // FIXME - find what to do here
            }

            public override float ScaleAbs
            {
                get { return Scale * ScaleModifier; }
            }

            public override float RotateAbs
            {
                get { return Rotate + RotateModifier; }
            }

            public override float ZoomAbs
            {
                get
                {
                    return Zoom /* + ZoomModifier */ ;
                }
            }
        }

        public RenderTarget2D RenderTarget
        {
            get
            {
                return renderTarget;
            }
            set
            {
                renderTarget = value;
                InitScreenDimensions();
            }
        }

        /// Width of visible screen in relative coordinates
        public float Width { get { return aspectRatio; } }

        /// Height of visible screen in relative coordinates
        public float Height { get { return 1.0f; } }

        /// Width of visible screen in pixels
        public int WidthPixels { get { return screenWidth; } }

        /// Height of visible screen in pixels
        public int HeightPixels { get { return screenHeight; } }

        /// Screen aspectratio 
        public float AspectRatio { get { return aspectRatio;  } }

        /// returns a single Rectangle instance with screen size/shape
        public Rectangle ScreenRectangle
        {
            get
            {
                return screenRect;
            }
        }


        #region Private and internal variables

        internal SpriteFont DebugFont = null;
        internal TTSpriteBatch mySpriteBatch = null;
        internal int screenWidth = 0;
        internal int screenHeight = 0;
        private float aspectRatio;
        internal float scalingToNormalized;
        private Rectangle screenRect;
        private RenderTarget2D renderTarget, effletRenderTarget;
        internal List<Efflet> effletsList;
        internal Dictionary<Effect, TTSpriteBatch> effect2spritebatchTable = new Dictionary<Effect, TTSpriteBatch>();
        internal List<TTSpriteBatch> spriteBatchesActive = new List<TTSpriteBatch>();
        
        #endregion

        #region Constructors

        /// <summary>
        /// create a Screenlet with no RenderTarget set (yet)
        /// </summary>
        public Screenlet()
        {
            Init();
            InitRenderTarget();
        }

        /// <summary>
        /// create a Screenlet with a RenderTarget buffer of given dimensions
        /// </summary>
        public Screenlet(int x, int y)
        {
            screenWidth = x;
            screenHeight = y;
            Init();
            InitRenderTarget();
        }

        #endregion

        public void DebugText(float x, float y, string text)
        {
            mySpriteBatch.DrawString(DebugFont, text, ToPixels(x, y), Color.White, 0f, Vector2.Zero, Motion.Zoom, SpriteEffects.None, 0f);
        }

        public void DebugText(Vector2 pos, string text)
        {
            DebugText(pos.X, pos.Y, text);
        }

        protected void Init()
        {
            TTengineMaster.ActiveScreen = this;
            Motion = new ScreenletMotion();
            Add(Motion);
            DrawInfo = new DrawComplet();
            Add(DrawInfo);
            Screen = this;
            TTengineMaster.AddScreenlet(this);
            DrawInfo.DrawColor = Color.Black; // for screen - default black background.
            graphicsDevice = TTengineMaster.ActiveGame.GraphicsDevice;
            try
            {
                // load the optional debug font.
                DebugFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>(@"TTDebugFont");
            }
            catch (ContentLoadException)
            {
                ; // TODO maybe put a warning out here?
            }
            effletsList = new List<Efflet>();
            mySpriteBatch = new TTSpriteBatch(graphicsDevice);
            collisionObjects = new List<Gamelet>();
        }

        protected void InitRenderTarget()
        {
            if (screenWidth > 0 && screenHeight > 0)  // init based on constructor parameters
                renderTarget = new RenderTarget2D(graphicsDevice, screenWidth, screenHeight);
            InitScreenDimensions(); // screenWidth / screenHeight are modified here
            effletRenderTarget = new RenderTarget2D(graphicsDevice, screenWidth, screenHeight);
        }

        protected void InitScreenDimensions()
        {
            if (renderTarget != null)
            {
                screenWidth = renderTarget.Width;
                screenHeight = renderTarget.Height;
            }
            else
            {
                screenWidth = graphicsDevice.Viewport.Width;
                screenHeight = graphicsDevice.Viewport.Height;
            }
            scalingToNormalized = 1.0f / (float)screenHeight;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);
            aspectRatio = (float)screenWidth / (float)screenHeight;
            Center = new Vector2(aspectRatio / 2.0f, 0.5f);
        }

        /// <summary>
        /// let the caller indicate that it wants to draw using the given shared SpriteBatch, which
        /// is not linked to any shader Effect. The SpriteBatch.Begin() method will be called if 
        /// needed here and also SpriteBatch.End() will be called by TTengine later after use.
        /// </summary>
        /// <param name="spb">spritebatch to request use of</param>
        internal void UseSharedSpriteBatch(TTSpriteBatch spb)
        {
            if (!spriteBatchesActive.Contains(spb))
            {
                spb.BeginParameterized();
                spriteBatchesActive.Add(spb);
            }            
        }

        /// <summary>
        /// let the caller indicate that it wants to draw using the default shared SpriteBatch, which
        /// is not linked to any shader Effect. The SpriteBatch.Begin() method will already have
        /// been called and also SpriteBatch.End() will be called by TTengine.
        /// </summary>
        internal TTSpriteBatch UseSharedSpriteBatch()
        {
            if (!spriteBatchesActive.Contains(mySpriteBatch))
            {
                mySpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatchesActive.Add(mySpriteBatch);
            }
            return mySpriteBatch;
        }

        /// <summary>
        /// Create (if needed) a shared spritebatch that uses shader Effect eff
        /// </summary>
        /// <param name="eff">shader Effect to create a shared TTSpriteBatch for</param>
        /// <returns>The created or fetched-from-cache (if already there) SpriteBatch</returns>
        internal TTSpriteBatch CreateSharedSpriteBatch(Effect eff)
        {
            TTSpriteBatch sb = null;
            try
            {
                sb = effect2spritebatchTable[eff];
            }
            catch (KeyNotFoundException)
            {
                // create it on 1st time
                sb = new TTSpriteBatch(graphicsDevice);
                effect2spritebatchTable[eff] = sb;
            }
            return sb;
        }
        
        /// <summary>
        /// let the caller indicate that it wants to draw using a shared TTSpriteBatch, where the
        /// TTSpriteBatch is associated to a shader Effect to be used while drawing. The TTSpriteBatch.BeginParameterized()
        /// method will already have been called and also TTSpriteBatch.End() will be called by TTengine.
        /// </summary>
        /// <param name="eff">the Effect linked to the shared TTSpriteBatch that the caller wishes to use</param>
        /// <returns>the shared TTSpriteBatch to be used for drawing</returns>
        internal TTSpriteBatch UseSharedSpriteBatch(Effect eff)
        {
            TTSpriteBatch sb = CreateSharedSpriteBatch(eff);
            if (!spriteBatchesActive.Contains(sb))
            {
                sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, eff);
                spriteBatchesActive.Add(sb);
            }
            return sb;
        }

        internal override void Draw(ref DrawParams p)
        {
            if (!Active) return;

            // check active in state
            if (ActiveInState != null)
            {
                if (!IsInState(ActiveInState))
                    return;
            }

            // start draw cycle by clearing efflets. Draw() call may spawn new efflets later.
            lock (graphicsDevice)
            {
                effletsList.Clear();
                spriteBatchesActive.Clear();

                // render my children to render buffer, in sprite order using depth info.
                RenderTargetBinding[] rts = null;
                if (renderTarget != null)
                {
                    rts = graphicsDevice.GetRenderTargets();
                    graphicsDevice.SetRenderTarget(renderTarget);
                }
                if (DrawInfo.Alpha > 0)   // only clear if background is not fully transparent
                    graphicsDevice.Clear(DrawInfo.DrawColor);

                // Draw() all children items:
                base.Draw(ref p);

                // close all remaining open effect-related spriteBatches
                foreach (SpriteBatch sb in spriteBatchesActive)
                    sb.End();

                // then apply all Efflets            
                if (effletsList.Count > 0)
                {
                    RenderTarget2D currentSourceBuffer = renderTarget;
                    RenderTarget2D currentTargetBuffer = effletRenderTarget;
                    if (renderTarget == null) throw new Exception("Efflets can only be used on Screenlets with a RenderTarget2D buffer set");
                    foreach (Efflet eff in effletsList)
                    {
                        graphicsDevice.SetRenderTarget(currentTargetBuffer);
                        eff.OnDrawEfflet(ref p, currentSourceBuffer); // apply eff to sourceBuffer

                        // Swap trick! for a next round.
                        RenderTarget2D temp = currentSourceBuffer;
                        currentSourceBuffer = currentTargetBuffer;
                        currentTargetBuffer = temp;
                    }
                    renderTarget = currentSourceBuffer;
                    effletRenderTarget = currentTargetBuffer;
                }

                // restore render target as it was before. TODO check if needed
                if (renderTarget != null)
                {
                    graphicsDevice.SetRenderTargets(rts); // restore from above changes
                }

                // render the renderTarget buffer, to screen if Visible
                if (Visible && renderTarget != null)
                {
                    mySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                    mySpriteBatch.Draw(renderTarget, ScreenRectangle, Color.White); // TODO may apply a selectable drawing color here?
                    mySpriteBatch.End();
                }
            }// end lock(graphicsDevice)
        }
    }
}
