// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TTengine
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
        public List<Spritelet> collisionObjects;

        public override Vector2 PositionAbsolute
        {
            get{ return Position + PositionModifier; }
        }

        public override float ScaleAbsolute
        {
            get{ return Scale * ScaleModifier; }
        }

        public override float RotateAbsolute
        {
            get{ return Rotate + RotateModifier; }
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
        public float Width { get { return aspectRatio / ScaleAbsolute; } }

        /// Height of visible screen in relative coordinates
        public float Height { get { return 1.0f / ScaleAbsolute; } }

        /// Width of visible screen in pixels
        public int WidthPixels { get { return screenWidth; } }

        /// Height of visible screen in pixels
        public int HeightPixels { get { return screenHeight; } }

        /// Screen aspectratio 
        public float AspectRatio { get { return aspectRatio;  } }

        /// TODO
        public float Zoom = 1.0f;

        /// returns a single Rectangle instance with screen size/shape
        public Rectangle ScreenRectangle
        {
            get
            {
                return screenRect;
            }
        }

        /// my spritebatch for drawing on this screen
        public SpriteBatch spriteBatch = null;

        private SpriteFont DebugFont = null;
        private int screenWidth;
        private int screenHeight;
        private float aspectRatio;
        private float scalingToNormalized;
        private Rectangle screenRect;
        private RenderTarget2D renderTarget, effletRenderTarget;
        internal List<Efflet> effletsList;

        /// create a Screenlet with no RenderTarget set (yet)
        public Screenlet(): base()
        {
            DrawColor = Color.Black; // for screen - default black background.
            graphicsDevice = TTengineMaster.ActiveGame.GraphicsDevice;
            InitScreenDimensions();
        }

        /// create a Screenlet with a RenderTarget buffer of given dimensions
        public Screenlet(int x, int y)
            : base()
        {
            DrawColor = Color.Black; // for screen - default black background.
            graphicsDevice = TTengineMaster.ActiveGame.GraphicsDevice;
            renderTarget = new RenderTarget2D(graphicsDevice, x, y);
            InitScreenDimensions();
        }

        public Vector2 ToPixels(Vector2 pos)
        {
            return pos * screenHeight * Zoom;
        }

        public Vector2 ToPixelsNS(Vector2 pos)
        {
            return pos * screenHeight ;
        }

        public Vector2 ToNormalized(Vector2 pos)
        {
            return pos * scalingToNormalized / Zoom;
        }

        public float ToPixels(float coord)
        {
            return coord * screenHeight * Zoom;
        }

        public Vector2 ToPixels(float x, float y)
        {
            return ToPixels(new Vector2(x,y));
        }

        public Vector2 ToPixelsNS(float x, float y)
        {
            return ToPixelsNS(new Vector2(x, y));
        }

        public float ToNormalized(float coord)
        {
            return coord * scalingToNormalized / Zoom;
        }

        public float ToNormalizedNS(float coord)
        {
            return coord * scalingToNormalized ;
        }

        public Vector2 ToNormalized(float x, float y)
        {
            return ToNormalized(new Vector2(x, y));
        }


        public void DebugText(float x, float y, string text)
        {
            spriteBatch.DrawString(DebugFont, text, ToPixels(x, y), Color.White, 0f, Vector2.Zero, Zoom, SpriteEffects.None, 0f);
        }

        public void DebugText(Vector2 pos, string text)
        {
            DebugText(pos.X, pos.Y, text);
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
            aspectRatio = (float) screenWidth / (float) screenHeight;
        }

        protected override void OnInit()
        {
            base.OnInit();

            DebugFont = TTengineMaster.ActiveGame.Content.Load<SpriteFont>(@"TTDebugFont");
            effletRenderTarget = new RenderTarget2D(graphicsDevice, screenWidth, screenHeight);
            effletsList = new List<Efflet>();
            spriteBatch = new SpriteBatch(graphicsDevice);
            InitScreenDimensions();
            collisionObjects = new List<Spritelet>();
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            //Scale = 1.2f;
            //Scale = (0.2f + 1f * (1f + (float)Math.Sin(MathHelper.TwoPi * 0.05f * SimTime)));
            // DEBUG
        }

        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                Zoom += 0.003f;
                Screen.DebugText(0.1f, 0.3f, "Zoom=" + Zoom);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                Zoom -= 0.003f;
                Screen.DebugText(0.1f, 0.3f, "Zoom=" + Zoom);
            }


        }

        internal override void Draw(ref DrawParams p)
        {
            if (!Active) return;
            // start draw cycle by clearing efflets. Draw() call may spawn new efflets later.
            effletsList.Clear();

            // render my children to render buffer, in sprite order using depth info.
            RenderTargetBinding[] rts = null;
            if (renderTarget != null)
            {
                rts = graphicsDevice.GetRenderTargets();
                graphicsDevice.SetRenderTarget(renderTarget);
            }
            graphicsDevice.Clear(this.drawColor);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);            
            // then Draw() all children items:
            base.Draw(ref p);            
            spriteBatch.End();

            // then apply all Efflets            
            if (effletsList.Count > 0)
            {
                RenderTarget2D currentSourceBuffer = renderTarget;
                RenderTarget2D currentTargetBuffer = effletRenderTarget;
                if  (renderTarget == null) throw new Exception("Efflets can only be used on Screenlets with a RenderTarget2D buffer set");
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

            // render the buffer, to screen if Visible
            if (Visible && renderTarget != null)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                spriteBatch.Draw(renderTarget, ScreenRectangle, Color.White);
                spriteBatch.End();
            }

        }
       
    }
}
