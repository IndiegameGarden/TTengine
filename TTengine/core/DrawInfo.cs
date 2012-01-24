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
        protected bool isDrawPositionCalculated = false;
        protected float width = 0f;
        protected float height = 0f;
        internal Color drawColor = Color.White;
        private const int NBUF = 10; // TODO tune?
        private Vector2[] posHistory = new Vector2[NBUF];
        private float[] posHistoryTime = new float[NBUF];
        private bool isFirstUpdatePosition = true;
        private int phIndex = 0;

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

        public Vector2 Center = new Vector2(0.5f, 0.5f); 

        public virtual float DrawScale { get { return Motion.ScaleAbs * Motion.ZoomAbs; } }

        public virtual Vector2 DrawCenter { get { return ToPixels(Center.X * width, Center.Y * height); } }

        public virtual float Width { get { return width; } set { width = value; } }

        public virtual float Height { get { return height; } set { height = value; } }

        public virtual float HeightAbs { get { return height * Motion.ScaleAbs; } }

        public virtual float WidthAbs { get { return width * Motion.ScaleAbs; } }

        public Vector2 DrawPosition
        {
            get
            {
                // if not yet calculated, return the current abs position as a best guess.
                if (!isDrawPositionCalculated)
                    return Motion.PositionAbs;
                return drawPosition;
            }
        }

        protected override void OnNewParent()
        {
            base.OnNewParent();
            // attach to parent's Motion object
            Motion = Parent.Motion;
        }

        // draw to this.screen at drawing pos
        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            if (Motion != null)
            {
                float t = (float)p.gameTime.TotalGameTime.TotalSeconds;
                // default - take latest position in cache
                drawPosition = DrawPosition;

                // then check if an interpolated, better value can be found.
                for (int i = 0; i < NBUF; i++)
                {
                    int iNext = (i + 1) % NBUF;
                    if (posHistoryTime[i] <= t && posHistoryTime[iNext] >= t)
                    {
                        float a = (t - posHistoryTime[i]) / (posHistoryTime[iNext] - posHistoryTime[i]);
                        // perform linear interpolation
                        drawPosition = ToPixels((1 - a) * posHistory[i] + a * posHistory[iNext]);
                        break;
                    }
                }
                isDrawPositionCalculated = true;
            }
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            if (Motion != null)
            {
                // store current position in a cache to use in trajectory smoothing
                UpdatePositionCache(Motion.PositionAbs, p.simTime);
            }
        }
        
        private void UpdatePositionCache(Vector2 updPos, float updTime)
        {
            if (isFirstUpdatePosition)
            {
                for (int i = 0; i < NBUF; i++)
                {
                    posHistory[i] = updPos;
                    posHistoryTime[i] = updTime;
                }
                isFirstUpdatePosition = false;
                phIndex = 0;
            }
            else
            {
                posHistory[phIndex] = updPos;
                posHistoryTime[phIndex] = updTime;
            }

            phIndex++;
            if (phIndex >= NBUF)
                phIndex = 0;
        }


    }
}
