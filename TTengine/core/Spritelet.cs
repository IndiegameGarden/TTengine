// (c) 2010-2012 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /**
     * a Spritelet represents a sprite with a rectangle or circle physical shape. It supports collission detection,
     * and has interpolation code for time-smoothed sprite drawing based on a fixed-timestep physics model.
     * Also it supports on-screen visibility detection. Shape dimension is supported by Width/Height/Radius
     * properties. 
     */
    public class Spritelet : Drawlet
    {
        #region Constructors

        /// <summary>
        /// create new spritelet without a Texture yet
        /// </summary>
        public Spritelet()
        {
        }

        /// <summary>
        /// create new spritelet where Texture is loaded from the content with given fileName
        /// </summary>
        /// <param name="fileName">name of XNA content file</param>
        public Spritelet(string fileName): base()
        {
            this.fileName = fileName;
            InitTextures();
        }

        /// <summary>
        /// create new spritelet with given Texture2D texture
        /// </summary>
        public Spritelet(Texture2D texture): base()
        {
            this.texture = texture;
            InitTextures();
        }

        #endregion

        #region Class-internal properties
        protected bool checksCollisions = false;
        protected string fileName = null;
        protected float radius = 0f;        
        #endregion

        #region Internal Vars (private)
        private Texture2D texture = null;
        private List<Spritelet> lastCollisionsList = new List<Spritelet>();
        #endregion

        #region Properties
        
        /// Flag indicating whether this item checks collisions with other Spritelets, by default false to save CPU
        public bool ChecksCollisions { 
            get { return checksCollisions;  } 
            set { 
                checksCollisions = value;
                if (checksCollisions && !Screen.collisionObjects.Contains(this))
                    Screen.collisionObjects.Add(this);
                if (!checksCollisions)
                    Screen.collisionObjects.Remove(this);
            } 
        }

        /// Check whether shape is visible on screen to the player(s) or not. If this.Visible is false, will return false always.
        public bool VisibleOnScreen
        {
            get
            {
                if (!Visible) return false;
                Vector2 p = Motion.PositionAbs;
                float w = DrawInfo.WidthAbs / 2;
                float h = DrawInfo.HeightAbs / 2;
                if (p.X < -w) return false;
                if (p.X > (Screen.Width + w)) return false;
                if (p.Y < -h) return false;
                if (p.Y > (1.0f + h)) return false;
                return true;
            }
        }

        /// Radius of item (if any) assuming a circular shape model
        public virtual float RadiusAbs { get { return radius * Motion.ScaleAbs; } }

        /**
         * get/set the Texture of this shape
         */
        public Texture2D Texture
        {
            set
            {
                texture = value;
                DrawInfo.Height = ToNormalizedNS(texture.Height );
                DrawInfo.Width = ToNormalizedNS(texture.Width);
                radius = DrawInfo.Width / 2;
            }
            get
            {
                return texture;
            }

        }

        #endregion

        protected virtual void InitTextures()
        {
            if (texture != null)
            {
                DrawInfo.Height = ToNormalizedNS(texture.Height);
                DrawInfo.Width = ToNormalizedNS(texture.Width);
                radius = DrawInfo.Width / 2;
            }
            if (fileName != null) 
                LoadTexture(fileName);
        }

        protected override void OnDelete()
        {
            Screen.collisionObjects.Remove(this);
        }

        /**
         * load  a (first-time, or new) texture for this shape)
         */
        protected void LoadTexture(string textureFilename)
        {
            Texture = TTengineMaster.ActiveGame.Content.Load<Texture2D>(textureFilename);
        }

        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            if (texture != null)
            {                
                MySpriteBatch.Draw(texture, DrawInfo.DrawPosition, null, DrawInfo.DrawColor,
                       Motion.RotateAbs, DrawInfo.DrawCenter, DrawInfo.DrawScale, SpriteEffects.None, DrawInfo.LayerDepth);
            }
        }

        /// Checks if there is a collision between the this and the passed in item. Default Spritelet implementation
        /// is a circular collision shape. Can be overridden with more complex detections.
        /// <returns>True if there is a collision, false if not</returns>
        public virtual bool CollidesWith(Spritelet item)
        {
            // simple sphere (well circle!) check
            if ((Motion.PositionAbs - item.Motion.PositionAbs).Length() < (RadiusAbs + item.RadiusAbs))
                return true;
            else
                return false;
        }

        /// run collision detection of this against all other relevant Spritelets
        internal void HandleCollisions(UpdateParams p)
        {
            if (!Active || !Visible ) return;

            // phase 1: check which items collide with me and add to list
            List<Spritelet> collItems = new List<Spritelet>();
            foreach (Spritelet s in Screen.collisionObjects)
            {
                if (s.Active  && s != this && 
                    CollidesWith(s) && s.CollidesWith(this) ) // a collision is detected
                {
                    collItems.Add(s);
                }
            }

            // phase 2: process the colliding items, to see if they are newly colliding or not
            foreach (Spritelet s in collItems)
            {
                if (!lastCollisionsList.Contains(s)) // if was not previously colliding, in previous round...
                {
                    // ... notify the collision methods
                    OnCollision(s);
                    OnCollideEventNotification(s);
                }
            }

            // phase 3: store list of colliding items for next round
            lastCollisionsList = collItems;
        }

        internal float ToNormalizedNS(float coord)
        {
            return coord * Screen.scalingToNormalized;
        }

    }
}
