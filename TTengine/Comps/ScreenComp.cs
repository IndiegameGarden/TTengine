// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TTengine.Core
{
    /// <summary>
    /// Component to turn an Entity into a 'Screenlet', which acts as a screen
    /// (RenderBuffer) where other entities can draw themselves to.
    /// <seealso cref="ScreenletSystem"/>
    /// </summary>
    public class ScreenComp: Comp
    {
        public Color BackgroundColor = Color.Black;

        public bool Visible = true;

        /// <summary>
        /// The center coordinate of the screenletEntity
        /// </summary>
        public Vector2 Center { get; private set; }

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

        /// Width of visible screenletEntity in relative coordinates
        public float Width { get { return aspectRatio; } }

        /// Height of visible screenletEntity in relative coordinates
        public float Height { get { return 1.0f; } }

        /// Width of visible screenletEntity in pixels
        public int WidthPixels { get { return screenWidth; } }

        /// Height of visible screenletEntity in pixels
        public int HeightPixels { get { return screenHeight; } }

        /// screenlet aspectratio 
        public float AspectRatio { get { return aspectRatio;  } }

        /// returns a single Rectangle instance with screenletEntity size/shape
        public Rectangle ScreenRectangle
        {
            get
            {
                return screenRect;
            }
        }


        #region Private and internal variables

        internal TTSpriteBatch SpriteBatch = null;
        internal int screenWidth = 0;
        internal int screenHeight = 0;
        private float aspectRatio;
        internal float scalingToNormalized;
        private Rectangle screenRect;
        private RenderTarget2D renderTarget;
        internal List<TTSpriteBatch> spriteBatchesActive = new List<TTSpriteBatch>();
        
        #endregion

        /// <summary>
        /// create a Screenlet of given dimensions with optionally a RenderTarget 
        /// </summary>
        public ScreenComp(bool hasRenderBuffer, int x, int y)
        {
            screenWidth = x;
            screenHeight = y;
            OnConstruction();
            if (hasRenderBuffer)
                InitRenderTarget();
            InitScreenDimensions();
        }

        /// <summary>
        /// create a Screenlet of full-screenletEntity dimensions with optionally a RenderTarget 
        /// </summary>
        public ScreenComp(bool hasRenderBuffer)
        {
            screenWidth = TTGame.Instance.GraphicsDevice.Viewport.Width;
            screenHeight = TTGame.Instance.GraphicsDevice.Viewport.Height;
            OnConstruction();
            if (hasRenderBuffer)
                InitRenderTarget();
            InitScreenDimensions();
        }

        protected void OnConstruction()
        {
            // TODO spritebatch can be supplied from outside? optimize TTGame.Instance.GraphicsDevice access?
            SpriteBatch = new TTSpriteBatch(TTGame.Instance.GraphicsDevice);
        }

        protected void InitRenderTarget()
        {
            if (screenWidth > 0 && screenHeight > 0)  // init based on constructor parameters
                renderTarget = new RenderTarget2D(TTGame.Instance.GraphicsDevice, screenWidth, screenHeight);
            else
                renderTarget = null;            
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
                screenWidth = TTGame.Instance.GraphicsDevice.Viewport.Width;
                screenHeight = TTGame.Instance.GraphicsDevice.Viewport.Height;
            }
            scalingToNormalized = 1.0f / (float)screenHeight;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);
            aspectRatio = (float)screenWidth / (float)screenHeight;
            Center = new Vector2(aspectRatio / 2.0f, 0.5f);
        }

    }
}
