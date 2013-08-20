// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace TTengine.Core
{
    /// <summary>
    /// Component for simple 2D circular physical shape collision detection.
    /// Requires: MotionComp
    /// </summary>
    public class CollisionComp : Comp
    {

        public CollisionComp()
        {
            Register(this);
            Screen = TTengineMaster.ActiveScreen;
        }

        #region Class-internal properties

        protected Screenlet Screen = null;
        protected bool checksCollisions = true;
        protected float radius = 0f;
        private List<Gamelet> lastCollisionsList = new List<Gamelet>();

        #endregion


        #region Properties
        
        /// Radius of item assuming a circular shape model
        public virtual float RadiusAbs { get { return radius * Parent.Motion.ScaleAbs; } }

        /// <summary>
        /// Radius used for collision detection range
        /// </summary>
        public float Radius { get { return radius; } set { radius = value; } }

        #endregion

        /// Checks if there is a collision between the this and another item. Default Spritelet implementation
        /// is a circular collision shape. Can be overridden with more complex detections.
        /// <returns>True if there is a collision, false if not</returns>
        public virtual bool CollidesWith(Gamelet item)
        {
            // simple sphere (well circle!) check
            if ((Parent.Motion.PositionAbs - item.Motion.PositionAbs).Length() < (RadiusAbs + item.Collision.RadiusAbs))
                return true;
            else
                return false;
        }

        /// run collision detection of this against all other relevant Spritelets
        protected void TodoDoCollission(ref UpdateParams p)
        {
            // phase 1: check which items collide with me and add to list
            List<Gamelet> collItems = new List<Gamelet>();
            foreach (Gamelet s in Screen.collisionObjects)
            {
                if (s.Active  && s != Parent &&
                    CollidesWith(s) && s.Collision.CollidesWith(Parent)) // a collision is detected
                {
                    collItems.Add(s);
                }
            }

            // phase 2: process the colliding items, to see if they are newly colliding or not
            foreach (Gamelet s in collItems)
            {
                if (!lastCollisionsList.Contains(s)) // if was not previously colliding, in previous round...
                {
                    // ... notify the collision methods
                    Parent.OnCollision(s);
                    Parent.OnCollideEventNotification(s);
                }
            }

            // phase 3: store list of colliding items for next round
            lastCollisionsList = collItems;
        }

    }
}
