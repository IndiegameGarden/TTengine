using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /// <summary>
    /// Component that allows drawing of the gamelet, offering very basic drawing functions
    /// </summary>
    public class DrawComplet: Complet
    {

        public DrawComplet()
            : base()
        {
            for (int i = 0; i < NBUF; i++)
            {
                posHistoryTime[i] = -1.0f; // init to out-of-scope value
            }
        }

        #region Internal vars
        
        protected Vector2 drawPosition = Vector2.Zero;
        protected float drawScale = 1f;
        protected bool isDrawPositionCalculated = false;
        internal Color drawColor = Color.White;
        internal TTSpriteBatch mySpriteBatch = null;
        private const int NBUF = 10; // TODO tune?
        private Vector2[] posHistory = new Vector2[NBUF];
        private float[] drawScaleHistory = new float[NBUF];
        private float[] posHistoryTime = new float[NBUF];
        private bool isFirstUpdatePosition = true;
        private uint phIndex = 0;

        #endregion


        /// <summary>
        /// a value indicating drawing depth of sprite 0f (front)....1f (back)
        /// </summary>
        public float LayerDepth = 0.5f;

        /// Color for drawing shape/sprite, setting it will replace Alpha value with DrawColor.A
        public virtual Color DrawColor
        {
            get { return drawColor; }
            set { drawColor = value; }
        }

        public virtual float R
        {
            get {return drawColor.R / 255.0f; }
            set { drawColor.R = (byte)(value * 255.0f); } 
        }

        public virtual float G
        {
            get { return drawColor.G / 255.0f; }
            set { drawColor.G = (byte)(value * 255.0f); }
        }

        public virtual float B
        {
            get { return drawColor.B / 255.0f; }
            set { drawColor.B = (byte)(value * 255.0f); }
        }

        /// Alpha value for the DrawColor of this Spritelet, range 0-1, replacing whatever was in DrawColor.A
        public virtual float Alpha
        {
            get { return drawColor.A / 255.0f; }
            set { drawColor.A = (byte)(value * 255.0f); }
        }

        /// <summary>
        /// returns a compound scale value for scaling use in Draw() calls, taking into account zoom as well.
        /// </summary>
        public virtual float DrawScaleCurrent { get { return Parent.Motion.ScaleAbs * Parent.Motion.ZoomAbs; } }

        public virtual float DrawScale
        {
            get
            {
                // if not yet calculated, return the current abs position as a best guess.
                if (!isDrawPositionCalculated)
                    return DrawScaleCurrent;
                return drawScale;
            }
        }

        /// <summary>
        /// an interpolated position in pixels for sprite drawing with smooth motion, directly usable in Draw() calls
        /// </summary>
        public Vector2 DrawPosition
        {
            get
            {
                // if not yet calculated, return the current abs position as a best guess.
                if (!isDrawPositionCalculated)
                    return Parent.Motion.PositionAbsZoomedPixels;
                return drawPosition;
            }
        }

        /// <summary>
        /// get the default TTSpriteBatch to use for drawing for this Gamelet. If not configured
        /// explicitly, it will use the default TTSpriteBatch of the Screenlet it renders to.
        /// </summary>
        public virtual TTSpriteBatch MySpriteBatch
        {
            get
            {
                Parent.Screen.UseSharedSpriteBatch(mySpriteBatch);
                return mySpriteBatch;
            }

            set
            {
                mySpriteBatch = value;
            }
        }

        // calculates drawing positions based on interpolation
        protected override void OnDraw(ref DrawParams p)
        {
            float t = (float)p.gameTime.TotalGameTime.TotalSeconds;
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

        protected override void OnUpdate(ref UpdateParams p)
        {
            UpdateSmoothingCache(ref p);
        }

        public override void OnNewParent()
        {
            if (Parent.Screen != null)
                mySpriteBatch = Parent.Screen.mySpriteBatch;
        }

        protected override void OnDelete()
        {
            //
        }

        internal void UpdateSmoothingCache(ref UpdateParams p)
        {
            // store current position in a cache to use in trajectory smoothing
            UpdatePositionCache(Parent.Motion.PositionAbsZoomedPixels, DrawScaleCurrent, p.SimTime);
        }
        
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
