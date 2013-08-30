using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine.Core;

using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Component that allows drawing of the gamelet, including basic drawing parameters
    /// like drawing position, layer depth, drawing color, to which Screenlet, etc
    /// </summary>
    public class DrawComp: IComponent
    {
        public DrawComp()
        {
            Screen = TTGame.Instance.ActiveScreen;
        }

        public DrawComp(ScreenletComp drawToScreen)
        {
            Screen = drawToScreen;
        }

        #region Internal vars
        
        protected Vector2 drawPosition = Vector2.Zero;
        protected float drawScale = 1f;
        protected bool isDrawPositionCalculated = false;
        internal Color drawColor = Color.White;
        internal TTSpriteBatch mySpriteBatch = null;
        private const int NBUF = 10; // TODO tune?
        private Vector2[] posHistory = new Vector2[NBUF];
        private double[] drawScaleHistory = new double[NBUF];
        private double[] posHistoryTime = new double[NBUF];
        private bool isFirstUpdatePosition = true;
        private uint phIndex = 0;

        #endregion

        /// <summary>Flag whether the Entity is visible (i.e. is being drawn or not)</summary>
        public bool IsVisible = true;

        /// <summary>drawing depth of graphics 0f (front)....1f (back)</summary>
        public float LayerDepth = 0.5f;

        /// <summary>to which Screenlet an Entity will be drawn</summary>
        public ScreenletComp Screen ;

        /// <summary>Color for drawing, setting this will replace Alpha value with DrawColor.A</summary>
        public virtual Color DrawColor
        {
            get { return drawColor; }
            set { drawColor = value; }
        }

        /// <summary>Red color component of DrawColor</summary>
        public virtual float R
        {
            get {return drawColor.R / 255.0f; }
            set { drawColor.R = (byte)(value * 255.0f); } 
        }

        /// <summary>Green color component of DrawColor</summary>
        public virtual float G
        {
            get { return drawColor.G / 255.0f; }
            set { drawColor.G = (byte)(value * 255.0f); }
        }

        /// <summary>Blue color component of DrawColor</summary>
        public virtual float B
        {
            get { return drawColor.B / 255.0f; }
            set { drawColor.B = (byte)(value * 255.0f); }
        }

        /// <summary>Alpha value for DrawColor, range 0f-1f, setting replaces DrawColor.A</summary>
        public virtual float Alpha
        {
            get { return drawColor.A / 255.0f; }
            set { drawColor.A = (byte)(value * 255.0f); }
        }

        /// <summary>Scale to used in Draw() calls</summary>
        public float DrawScale = 1.0f;

        /// <summary>
        /// position in pixels for drawing, directly usable in Draw() calls
        /// </summary>
        public Vector2 DrawPosition
        {
            get; set;
        }

        // calculates drawing positions based on interpolation
        /*
        public override void OnDraw(ref DrawParams ctx)
        {
            float t = (float)ctx.gameTime.TotalGameTime.TotalSeconds;
            // default - take latest position in cache
            drawPosition = DrawPosition;
            drawScale = DrawScaleCurrent;

            // then check if an interpolated, better value can be found.
            for (uint i = 0; i < NBUF; i++)
            {
                uint iNext = (i + 1) % NBUF;
                if (posHistoryTime[i] <= t && posHistoryTime[iNext] >= t)
                {
                    float a = (t - posHistoryTime[i]) / (posHistoryTime[iNext] - posHistoryTime[i]);
                    // perform linear interpolation
                    drawPosition = (1 - a) * posHistory[i] + a * posHistory[iNext];
                    drawScale = (1 - a) * drawScaleHistory[i] + a * drawScaleHistory[iNext];
                    break;
                }
            }
            isDrawPositionCalculated = true;
        }
         */

        /*
        protected override void OnUpdate(ref UpdateParams ctx)
        {
            UpdateSmoothingCache(ref ctx);
        }
         */

        /// <summary>
        /// FIXME move away to a component?
        /// translate a float screenletEntity coordinate to pixel coordinates, in the context of this Gamelet
        /// </summary>
        /// <param name="pos">relative coordinate to translate</param>
        /// <returns>translated to pixels coordinate</returns>
        public Vector2 ToPixels(Vector2 pos)
        {
            //return (pos * Screen.screenHeight - Center) * Zoom + Center; // TODO check? only for internal?
            return pos * Screen.screenHeight;
        }

        public float ToPixels(float coord)
        {
            return coord * Screen.screenHeight;
        }

        public float FromPixels(float pixels)
        {
            return pixels / Screen.screenHeight;
        }

        public Vector2 FromPixels(Vector2 pixelCoords)
        {
            return pixelCoords / Screen.screenHeight;
        }

        internal Vector2 ToPixels(float x, float y)
        {
            return ToPixels(new Vector2(x, y));
        }

        /*
        internal void UpdateSmoothingCache(ref UpdateParams ctx)
        {
            // store current position in a cache to use in trajectory smoothing
            UpdatePositionCache(Parent.Motion.PositionAbsZoomedPixels, DrawScaleCurrent, ctx.SimTime);
        }

        internal void VertexShaderInit(Effect eff)
        {
            // vertex shader init            
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Screen.WidthPixels, Screen.HeightPixels, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            Matrix m = halfPixelOffset * projection;
            EffectParameter mtPar = eff.Parameters["MatrixTransform"];
            if (mtPar != null)
                mtPar.SetValue(m);
        }
        */

        private void UpdatePositionCache(Vector2 updPos, float updDrawScale, float updTime)
        {
            if (isFirstUpdatePosition)
            {
                for (uint i = 0; i < NBUF; i++)
                {
                    posHistory[i] = updPos;
                    posHistoryTime[i] = updTime;
                    drawScaleHistory[i] = updDrawScale;
                }
                isFirstUpdatePosition = false;
                phIndex = 0;
            }
            else
            {
                posHistory[phIndex] = updPos;
                posHistoryTime[phIndex] = updTime;
                drawScaleHistory[phIndex] = updDrawScale;
            }

            phIndex++;
            if (phIndex == NBUF)
                phIndex = 0;
        }


    }
}
