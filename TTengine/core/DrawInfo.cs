using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    public class DrawInfo: Gamelet
    {
        protected Vector2 drawPosition = Vector2.Zero;
        protected float drawScale = 1f;
        protected bool isDrawPositionCalculated = false;
        protected float width = 0f;
        protected float height = 0f;
        internal Color drawColor = Color.White;
        private const int NBUF = 10; // TODO tune?
        private Vector2[] posHistory = new Vector2[NBUF];
        private float[] drawScaleHistory = new float[NBUF];
        private float[] posHistoryTime = new float[NBUF];
        private bool isFirstUpdatePosition = true;
        private uint phIndex = 0;

        /// a value indicating drawing depth of sprite 0f (front)....1f (back)
        public float LayerDepth = 0.5f;
        
        /// Color for drawing shape/sprite, setting it will replace Alpha value with DrawColor.A
        public virtual Color DrawColor
        {
            get { return drawColor; }
            set { drawColor = value; }
        }

        /// Alpha value for the DrawColor of this Spritelet, range 0-1, replacing whatever was in DrawColor.A
        public virtual float Alpha
        {
            get { return drawColor.A / 255.0f; }
            set { drawColor.A = (byte)(value * 255.0f); }
        }

        /// <summary>
        /// Center of sprite expressed in relative width/height coordinates, where 1.0 is full width or full height.
        /// By default the center of the sprite is chosen in the middle.
        /// </summary>
        public Vector2 Center = new Vector2(0.5f, 0.5f); 

        /// <summary>
        /// returns a compound scale value for scaling use in Draw() calls, taking into account zoom as well.
        /// </summary>
        public virtual float DrawScaleCurrent { get { return Motion.ScaleAbs * Motion.ZoomAbs; } }

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
        /// calculates a center coordinate for direct use in Draw() calls, expressed in pixels
        /// </summary>
        public virtual Vector2 DrawCenter { get { return ToPixels(Center.X * width, Center.Y * height); } }

        /// <summary>
        /// relative width of sprite in normalized coordinates
        /// </summary>
        public virtual float Width { get { return width; } set { width = value; } }

        /// <summary>
        /// relative height of sprite in normalized coordinates
        /// </summary>
        public virtual float Height { get { return height; } set { height = value; } }

        /// <summary>
        /// absolute width of sprite, being Width after applying any scaling
        /// </summary>
        public virtual float WidthAbs { get { return width * Motion.ScaleAbs; } }

        /// <summary>
        /// absolute height of sprite, being Height after applying any scaling
        /// </summary>
        public virtual float HeightAbs { get { return height * Motion.ScaleAbs; } }

        /// <summary>
        /// an interpolated position in pixels for sprite drawing with smooth motion, directly usable in Draw() calls
        /// </summary>
        public Vector2 DrawPosition
        {
            get
            {
                // if not yet calculated, return the current abs position as a best guess.
                if (!isDrawPositionCalculated)
                    return Motion.PositionAbsZoomedPixels;
                return drawPosition;
            }
        }

        protected override void OnNewParent()
        {
            base.OnNewParent();
            // attach to parent's Motion object
            Motion = Parent.Motion;
        }

        // calculates drawing positions based on interpolation
        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

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
            base.OnUpdate(ref p);

            // store current position in a cache to use in trajectory smoothing
            UpdatePositionCache(Motion.PositionAbsZoomedPixels, DrawScaleCurrent, p.SimTime);
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
