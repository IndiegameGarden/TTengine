﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine.Core;
using Artemis;
using Artemis.Interface;


namespace TTengine.Comps
{
    /// <summary>
    /// Component that allows drawing of the gamelet, including basic drawing parameters
    /// like drawing position, layer depth, drawing color, etc
    /// </summary>
    public class DrawComp: IComponent
    {
        /// <summary>
        /// Create new DrawComp
        /// </summary>
        /// <param name="drawScreen">ScreenComp that the Entity will be drawn to, or null to use the channel's
        /// default.</param>
        public DrawComp(ScreenComp drawScreen = null)
        {
            this.DrawScreen = drawScreen;
        }

        /// <summary>
        /// Screen to draw this Entity exclusively to, or null to draw to the default screen of the channel.
        /// By default null.
        /// </summary>
        public ScreenComp DrawScreen = null;

        /// <summary>Flag whether the Entity is visible currently (i.e. is being drawn or not)</summary>
        public bool IsVisible = true;

        /// <summary>Z coordinate and drawing depth of graphics 0f (front)....1f (back). WARNING: this value is overwritten
        /// each update by various Systems. Use PositionComp.Depth to modify item depth.</summary>
        public float LayerDepth
        {
            get { return DrawPosition.Z; }
            internal set { DrawPosition.Z = value; }
        }

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

        /// <summary>Rotation to use in Draw() calls</summary>
        public float DrawRotation = 0f;

        /// <summary>
        /// position coordinate x,y,z for drawing, WARNING: is being set by systems like SpriteRenderSystem on each draw.
        /// </summary>
        public Vector3 DrawPosition;

        /// <summary>
        /// position coordinate x,y for drawing, directly usable in Draw() calls. WARNING: is being set by systems like SpriteRenderSystem on each draw.
        /// </summary>
        public Vector2 DrawPositionXY
        {
            get { return new Vector2(DrawPosition.X, DrawPosition.Y); }
        }

        // internal vars
        internal Color drawColor = Color.White;

    }
}
