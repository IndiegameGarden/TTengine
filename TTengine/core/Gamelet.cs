// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /// <summary>
    /// Generic event arguments class to be used for Gamelet related events
    /// </summary>
    public class GameletEventArgs : EventArgs
    {
        /// <summary>
        /// describes item that is subject of event (e.g. colliding Gamelet)
        /// </summary>
        public Gamelet otherItem;

        public GameletEventArgs(Gamelet otherItem)
        {
            this.otherItem = otherItem;
        }
        
    }

    /// <summary>
    /// Generic event handler
    /// </summary>
    /// <param name="sender">generator of event</param>
    /// <param name="e">see above class, which contains any event arguments</param>
    public delegate void GameletEventHandler(Gamelet sender, GameletEventArgs e);

    /**
     * basic game entity class that can refer to children entities. 
     * TODO: ? Provides subclass extendibility via On...() methods.
     */
    public class Gamelet: Complet
    {

        #region Eventing
        public event GameletEventHandler OnCollisionEvent;
        #endregion
        
        #region Properties

        /// <summary>
        /// Children of this gamelet
        /// </summary>
        public List<Gamelet> Children = new List<Gamelet>();

        /// <summary>
        /// total cumulative amount of simulation time of this gamelet
        /// </summary>
        public float SimTime = 0f;

        /// <summary>
        /// whether this Gamelet is active; if not updates for it will not be done
        /// </summary>
        public bool Active = true;

        /// <summary>
        /// Flag indicating if Gamelet should be deleted - if true deletion occurs during next Update
        /// </summary>
        public bool Delete = false;

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public Gamelet()
        {
            //
        }

        #region Overridable Handler methods (On...() methods)

        public virtual void OnDelete()
        {
            //
        }

        public virtual void OnInit()
        {
            //
        }

        public virtual void OnNewParent(Gamelet oldParent)
        {
            //
        }

        #endregion

        #region Children methods
        /// <summary>
        /// Adds a Gamelet as child. 
        /// Note: Does not add again if 'child' is already a child Gamelet of this one.
        /// </summary>
        public void Add(Gamelet child)
        {
            if (!Children.Contains(child))
            {
                Gamelet oldParent = child.Parent;
                child.Parent = this;
                Children.Add(child);
                child.OnNewParent(oldParent);
            }
        }

        /// <summary>
        /// Removes specified child gamelet from the list of children. The child.OnNewParent() is
        /// called with a null parameter, indicating it is parentless.
        /// </summary>
        /// <returns>true if item is successfully removed; otherwise, false. </returns>
        public bool Remove(Gamelet child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                Gamelet oldParent = child.Parent;
                child.Parent = null;
                child.OnNewParent(oldParent);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
