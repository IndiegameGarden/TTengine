// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Artemis.Interface;

namespace TTengine.Core
{
    /// <summary>
    /// Screen to which graphics are rendered, optionally containing a separate RendereBuffer
    /// <seealso cref="ScreenletSystem"/>
    /// </summary>
    public class ScreenComp: IComponent
    {
        /// <summary>create a ScreenComp of given dimensions with optionally a RenderTarget.
        /// If (0,0) given, uses default backbuffer size </summary>
        protected ScreenComp(bool hasRenderBuffer, int x, int y)
        {
            SpriteBatch = new TTSpriteBatch(TTGame.Instance.GraphicsDevice);
            if (hasRenderBuffer)
                renderTarget = new RenderTarget2D(TTGame.Instance.GraphicsDevice, x, y);
            InitScreenDimensions();
        }

        /// <summary>
        /// Create with RenderTarget
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public ScreenComp(int x, int y):
            this(true,x,y)
        {
            // see this() constructor
        }

        /// <summary>Create without RenderTarget</summary>
        public ScreenComp(): 
            this(false,0,0)
        {
            // see this() constructor
        }

        public Color BackgroundColor = Color.Black;

        public bool IsVisible = true;

        /// <summary>The center pixel coordinate of the screen</summary>
        public Vector3 Center { get; private set; }

        /// <summary>The zoom-in factor, used for showing part of a screen and for translation of other coordinate systems to pixel coordinates.</summary>
        public float Zoom;

        /// <summary>The center coordinate, in either pixel or custom coordinates, for applying Zoom</summary>
        public Vector3 ZoomCenter;

        /// <summary>Get the RenderTarget. If null, the screen renders to the default backbuffer.
        /// </summary>
        public RenderTarget2D RenderTarget
        {
            get
            {
                return renderTarget;
            }
        }

        /// <summary>Width of screen in pixels</summary>
        public int Width { get { return screenWidth; } }

        /// <summary>Height of screen in pixels</summary>
        public int Height { get { return screenHeight; } }

        /// <summary>Screen aspectratio</summary> 
        public float AspectRatio { get { return aspectRatio;  } }

        /// <summary>The default spritebatch associated to this screen, for drawing to it</summary>
        public TTSpriteBatch SpriteBatch = null;

        #region Private and internal variables        
        //
        private int screenWidth = 0;
        private int screenHeight = 0;
        private float aspectRatio;
        private RenderTarget2D renderTarget;
        #endregion

        /// <summary>
        /// translate a Vector2 relative coordinate to pixel coordinates
        /// </summary>
        /// <param name="pos">relative coordinate to translate</param>
        /// <returns>translated to pixels coordinate</returns>
        public Vector2 ToPixels(Vector3 pos)
        {
            var v = (pos - ZoomCenter) * Zoom + Center;
            return new Vector2(v.X, v.Y);
            //return pos * screen.screenHeight;
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
                screenWidth = TTGame.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
                screenHeight = TTGame.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
            }

            aspectRatio = (float)screenWidth / (float)screenHeight;
            Center = new Vector3(((float)screenWidth)/2.0f, ((float)screenHeight)/2.0f, 0f);
            Zoom = 1f;
            ZoomCenter = Center;
        }

    }
}
