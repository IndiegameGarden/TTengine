using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /// <summary>
    /// Component that allows drawing of the gamelet, offering very basic drawing functions
    /// </summary>
    public class DrawComp: Comp
    {

        public DrawComp()
        {
            Register(this);
            Screen = TTengineMaster.ActiveScreen;
            for (int i = 0; i < NBUF; i++)
            {
                posHistoryTime[i] = -1.0; // init to out-of-scope value
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
        private double[] drawScaleHistory = new double[NBUF];
        private double[] posHistoryTime = new double[NBUF];
        private bool isFirstUpdatePosition = true;
        private uint phIndex = 0;

        #endregion


        /// <summary>
        /// a value indicating drawing depth of sprite 0f (front)....1f (back)
        /// </summary>
        public float LayerDepth = 0.5f;

        /// <summary>
        /// to which Screenlet the item belongs (e.g. where a shape will draw itself). Also non-drawables may use this info.
        /// Null if not set yet or unknown.
        /// </summary>
        public Screenlet Screen = null;

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

        public double DrawScale = 1.0;

        public double DrawScaleAbs = 1.0;

        /// <summary>
        /// an interpolated position in pixels for sprite drawing with smooth motion, directly usable in Draw() calls
        /// </summary>
        public Vector2 DrawPosition
        {
            get
            {
                return drawPosition;
            }
        }

        /// <summary>
        /// TODO needed?
        /// get the default TTSpriteBatch to use for drawing for this Gamelet. If not configured
        /// explicitly, it will use the default TTSpriteBatch of the Screenlet it renders to.
        /// </summary>
        public virtual TTSpriteBatch MySpriteBatch
        {
            get
            {
                Screen.UseSharedSpriteBatch(mySpriteBatch);
                return mySpriteBatch;
            }

            set
            {
                mySpriteBatch = value;
            }
        }

        // calculates drawing positions based on interpolation
        /*
        public override void OnDraw(ref DrawParams p)
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
         */

        /*
        protected override void OnUpdate(ref UpdateParams p)
        {
            UpdateSmoothingCache(ref p);
        }
         */

        public void OnNewParent(Gamelet oldParent)
        {
            if (Screen != null)
                mySpriteBatch = Screen.mySpriteBatch;
        }

        /*
        /// <summary>
        /// FIXME move away to a component?
        /// translate a float screen coordinate to pixel coordinates, in the context of this Gamelet
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
        */

        /*
        internal void UpdateSmoothingCache(ref UpdateParams p)
        {
            // store current position in a cache to use in trajectory smoothing
            UpdatePositionCache(Parent.Motion.PositionAbsZoomedPixels, DrawScaleCurrent, p.SimTime);
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
