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
     * basic game-object class that can refer to children entities and
     * can contain components (Comps).
     * TODO: ? Provides subclass extendibility via On...() methods.
     */
    public class Gamelet
    {

        public Gamelet()
        {
            CreateID();
        }

        #region Eventing
        public event GameletEventHandler OnCollisionEvent;
        #endregion
        
        #region Properties

        /// <summary>
        /// get the unique ID of this object
        /// </summary>
        public int ID { get { return _ID; } }

        /// <summary>
        /// the parent Gamelet (entity) that this Complet (component) is attached to, or null if none
        /// </summary>
        public Gamelet Parent = null;

        /// <summary>
        /// Children of this gamelet
        /// </summary>
        public List<Gamelet> Children = new List<Gamelet>();

        public List<Comp> Complets = new List<Comp>();

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

        public void AddComp(Comp child)
        {
            if (!Complets.Contains(child))
            {
                //Gamelet oldParent = child.Parent;
                child.Parent = this;
                Complets.Add(child);
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

        public bool RemoveComp(Comp child)
        {
            if (Complets.Contains(child))
            {
                Complets.Remove(child);
                child.Parent = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Internal Vars and methods

        private int _ID = -1;
        private static int _IDcounter = 0;

        private void CreateID()
        {
            // set my unique id
            _ID = _IDcounter;
            _IDcounter++;
        }

        #endregion

    }
}
