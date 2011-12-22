// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

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
    public class Spritelet : Gamelet
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
        protected Vector2 drawPosition = Vector2.Zero;
        protected bool drawPositionCalculated = false;
        protected bool checksCollisions = false;
        protected string fileName = null;
        protected float width = 0f;
        protected float height = 0f;
        protected float radius = 0f;        
        #endregion

        #region Internal Vars (private)
        private Texture2D texture = null;
        private const int NBUF = 10; // TODO tune?
        private Vector2[] posHistory = new Vector2[NBUF];
        private float[] posHistoryTime = new float[NBUF];
        private bool isFirstUpdatePosition = true;
        private int phIndex = 0;
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
                Vector2 p = PositionAbs;
                float w = WidthAbs / 2;
                float h = HeightAbs / 2;
                if (p.X < -w) return false;
                if (p.X > (Screen.Width + w)) return false;
                if (p.Y < -h) return false;
                if (p.Y > (1.0f + h)) return false;
                return true;
            }
        }

        /// Radius of item (if any) assuming a circular shape model
        public virtual float RadiusAbs { get { return radius * ScaleAbs; } }

        public virtual float HeightAbs { get { return height * ScaleAbs; } }

        public virtual float WidthAbs { get { return width * ScaleAbs; } }

        public virtual float DrawScale { get { return ScaleAbs * ZoomAbs ; } }

        public Vector2 Center = new Vector2(0.5f,0.5f); 
        public virtual Vector2 DrawCenter { get { return ToPixels(Center.X * width, Center.Y * height); } }

        public override Vector2 DrawPosition
        {
            get
            {
                if (!drawPositionCalculated)
                    return base.DrawPosition;
                return drawPosition;
            }
        }

        /**
         * get/set the Texture of this shape
         */
        public Texture2D Texture
        {
            set
            {
                texture = value;
                height = ToNormalizedNS(texture.Height );
                width = ToNormalizedNS(texture.Width);
                radius = width / 2;
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
                height = ToNormalizedNS(texture.Height);
                width = ToNormalizedNS(texture.Width);
                radius = width / 2;
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

        internal override void Update(ref UpdateParams p)
        {
            base.Update(ref p);

            if (Active)
            {
                // store current position in a cache to use in trajectory smoothing
                UpdatePositionCache(PositionAbs, p.simTime);
                // handle collisions
                if (checksCollisions) 
                    HandleCollisions(p);
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

        // draw to this.screen at drawing pos
        internal override void Draw(ref DrawParams p)
        {
            if (!Active) return;

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
            drawPositionCalculated = true;

            // do actual drawing of my children and then OnDraw()
            base.Draw(ref p);

        }

        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            if (texture != null)
            {                
                Screen.UseSharedSpritebatch().Draw(texture, DrawPosition, null, drawColor,
                       RotateAbs, DrawCenter, DrawScale, SpriteEffects.None, LayerDepth);
            }
        }

        /// Checks if there is a collision between the this and the passed in item. Default Spritelet implementation
        /// is a circular collision shape. Can be overridden with more complex detections.
        /// <returns>True if there is a collision, false if not</returns>
        public virtual bool CollidesWith(Spritelet item)
        {
            // simple sphere (well circle!) check
            if ((PositionAbs - item.PositionAbs).Length() < (RadiusAbs + item.RadiusAbs))
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
    }
}
