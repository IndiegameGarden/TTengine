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
    /// like drawing position, layer depth, drawing color, etc
    /// </summary>
    public class DrawComp: IComponent
    {
        public DrawComp()
        {            
        }

        #region Internal vars
        
        protected Vector2 drawPosition = Vector2.Zero;
        protected float drawScale = 1f;
        protected bool isDrawPositionCalculated = false;
        internal Color drawColor = Color.White;
        //internal TTSpriteBatch mySpriteBatch = null;
        private const int NBUF = 10; // TODO tune?
        private Vector2[] posHistory = new Vector2[NBUF];
        private double[] drawScaleHistory = new double[NBUF];
        private double[] posHistoryTime = new double[NBUF];
        //private bool isFirstUpdatePosition = true;
        //private uint phIndex = 0;

        #endregion

        /// <summary>Flag whether the Entity is visible (i.e. is being drawn or not)</summary>
        public bool IsVisible = true;

        /// <summary>drawing depth of graphics 0f (front)....1f (back)</summary>
        public float LayerDepth = 0.5f;

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

        /// <summary>Scale to use in Draw() calls</summary>
        public float DrawScale = 1.0f;

        /// <summary>
        /// position in pixels for drawing, directly usable in Draw() calls
        /// </summary>
        public Vector2 DrawPosition
        {
            get; set;
        }

        /// <summary>
        /// FIXME move away to a component?
        /// translate a float screenletEntity coordinate to pixel coordinates, in the context of this Gamelet
        /// </summary>
        /// <param name="pos">relative coordinate to translate</param>
        /// <returns>translated to pixels coordinate</returns>
        public Vector2 ToPixels(Vector2 pos)
        {
            //return (pos * screenlet.screenHeight - Center) * Zoom + Center; // TODO check? only for internal?
            // TODO optimize screenletcomp access
            return pos * TTGame.Instance.ActiveScreen.GetComponent<ScreenComp>().screenHeight;
        }

    }
}
