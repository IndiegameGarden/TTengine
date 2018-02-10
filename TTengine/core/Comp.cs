using System;
using System.Collections.Generic;
using Artemis.Interface;

namespace TTengine.Core
{
    /// <summary>
    /// optional base class for any components that implement IComponent. It provides 
    /// parent/child component relations.
    /// TODO can be extended with OnNewParent() event.
    /// </summary>
    public abstract class Comp: IComponent
    {
        /// <summary>Children components of this component; if null there are none</summary>
        public List<Comp> Children = null;

        private Comp _parent = null;

        /// <summary>
        /// The parent component of this one, or null if none.
        /// </summary>
        public Comp Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Add a new Comp as a child of this one. If c already is a child of someone, it gets removed as child there first. 
        /// </summary>
        /// <param name="c">The Comp to add as child.</param>
        public void AddChild(Comp c)
        {
            if (Children == null)
                Children = new List<Comp>();

            if (!Children.Contains(c))
            {
                if (c._parent != null && c._parent != this)
                    c._parent.RemoveChild(c);
                Children.Add(c);
            }
            c._parent = this;
        }

        public void RemoveChild(Comp c)
        {
            if (Children == null)
                return;
            if (!Children.Contains(c))
                return;
            Children.Remove(c);
            c._parent = null;
        }

    }
}
