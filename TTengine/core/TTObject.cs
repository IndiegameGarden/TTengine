// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{

    /// <summary>
    /// base class for most objects in TTengine
    /// </summary>
    public abstract class TTObject : IDisposable
    {

        #region Properties

        /// <summary>
        /// get the unique ID of this object
        /// </summary>
        public int ID     { get { return _ID; } }
        
        /// <summary>
        /// Flag indicating if Gamelet should be deleted - if true deletion occurs during next Update
        /// </summary>
        public bool Delete = false;

        /// <summary>
        /// whether this Gamelet is active; if not: OnUpdate() and OnDraw() will not be called
        /// </summary>
        public bool Active = true;

        public TTObject ParentObj = null ;
        public Gamelet Parent
        {
            get { return ParentObj as Gamelet; }
        }

        public List<TTObject> Children = new List<TTObject>();

        protected List<TTObject> childrenToAdd = new List<TTObject>();

        #endregion

        #region Constructors
        
        /// <summary>
        /// constructor
        /// </summary>
        public TTObject()
        {
            CreateID();
        }

        #endregion

        #region Internal Vars        
        private int _ID = -1;
        private static int _IDcounter = 0;
        #endregion

        #region Handler methods (On...() methods)

        /// <summary>
        /// called when parent changes, check Parent member for the new parent.
        /// </summary>
        public abstract void OnNewParent();

        /// <summary>
        /// called on TTObject tree init
        /// </summary>
        public abstract void OnInit();

        /// <summary>
        /// called on update of parent Gamelet, may be overridden to define custom Update behaviour
        /// </summary>
        protected abstract void OnUpdate(ref UpdateParams p);

        /// <summary>
        /// Called on drawing of the parent Gamelet
        /// </summary>
        public virtual void OnDraw(ref DrawParams p)
        {
            //
        }
    
        /// <summary>
        /// Called on moment of parent Gamelet deletion
        /// </summary>
        protected virtual void OnDelete()
        {
            //
        }

        #endregion

        /// <summary>
        /// Implements IDisposable to instantly stop and free up all unmanaged resources
        /// </summary>
        public virtual void Dispose()
        {
            DeleteItem();
        }

        #region Children methods
        /// <summary>
        /// Adds a Gamelet as child at the _end_ (back) of the children's list this.Children. Does not add
        /// if 'child' is already a child Gamelet of this one.
        /// </summary>
        public void Add(TTObject child)
        {
            if (!Children.Contains(child))
            {
                child.ParentObj = this;
                Children.Add(child);
                child.OnNewParent();
            }
        }

        /// <summary>
        /// add a child gamelet with Add() next update, useful for adding gamelet from another thread
        /// </summary>
        /// <param name="child"></param>
        public void AddNextUpdate(TTObject child)
        {
            lock (childrenToAdd)
            {
                childrenToAdd.Add(child);
            }
        }

        /// <summary>
        /// Adds (inserts) a Gamelet as child in _begin_ i.e. front of the children's list this.Children
        /// </summary>
        public void AddFront(TTObject child)
        {
            Children.Insert(0, child);
            child.ParentObj = this;
            child.OnNewParent();
        }

        public void Add(int index, TTObject child)
        {
            Children.Insert(index, child);
            child.ParentObj = this;
            child.OnNewParent();
        }

        /// <summary>
        /// Removes first occurrence of child gamelet from the list of children. The child.OnNewParent() is
        /// called with a null parameter as a result.
        /// </summary>
        /// <returns>true if item is successfully removed; otherwise, false. </returns>
        public bool Remove(TTObject child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.ParentObj = null;
                child.OnNewParent();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Private (internal) methods

        private void CreateID()
        {
            // set my unique id
            _ID = _IDcounter;
            _IDcounter++;
        }

        internal virtual void Init()
        {
            foreach (TTObject item in Children)
            {
                item.Init();
            }

            // call custom handler of current object 
            OnInit();
        }

        internal virtual void Update(ref UpdateParams p)
        {
            // add any children that were queued to add
            lock (childrenToAdd)
            {
                foreach (Gamelet g in childrenToAdd)
                    Add(g);
                childrenToAdd.Clear();
            }

            // simulate object and children
            //Remove any items that need deletion etc
            int i = 0;
            while (i < Children.Count)
            {
                // deleted items
                if (Children[i].Delete)
                {
                    Children[i].DeleteItem();
                    Children.RemoveAt(i);
                }
                // remove items from my tree that were transferred to another parent
                else if (Children[i].ParentObj != this)
                    Children.RemoveAt(i);
                else
                    i++;
            }

            //Update each child item. Note each child _may_ modify updPars.
            foreach (TTObject item in Children)
            {
                item.Update(ref p);
            }

            // call custom update handler of current object 
            if (Active)
                OnUpdate(ref p);

        }

        /// Called for deletion of this item - includes deletion of all children and calling OnDeletion
        internal virtual void DeleteItem()
        {
            Delete = true;
            int i = 0;
            while (i < Children.Count)
            {
                Children[i].Delete = true;
                Children[i].DeleteItem();
                i++;
            }
            Children.Clear();
            childrenToAdd.Clear();
            OnDelete();
        }

        #endregion
    }
}
