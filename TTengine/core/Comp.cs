using System;
using System.Collections.Generic;
using Artemis.Interface;

namespace TTengine.Core
{
    /// <summary>
    /// optional base class for components that implement IComponent. This provides 
    /// parent/child component relations.
    /// </summary>
    public abstract class Comp: IComponent
    {
        /// <summary>Children components of this component, if null there are none (so far)</summary>
        public List<Comp> Children = null;

        /// <summary>
        /// The parent component of this one, or null if none. Changing this field is 
        /// done automatically via AddChild()
        /// </summary>
        public Comp Parent
        {
            get { return _parent; }
        }

        private Comp _parent = null;

        /// <summary>
        /// Add a new Comp as a child of this one.
        /// </summary>
        /// <param name="child">The child Comp to add.</param>
        public void AddChild(Comp child)
        {
            if (Children == null)
                Children = new List<Comp>();

            if (!Children.Contains(child))
            {
                Children.Add(child);
            }
            child._parent = this;
        }

    }
}
