using System;
using System.Collections.Generic;

using System.Text;

using Artemis.Interface;
using Artemis.System;

namespace TTengine.Core
{
    /// <summary>
    /// optional base class for components that implement IComponent. This provides SimTime time
    /// counter and optional parent/child component relations.
    /// </summary>
    public abstract class Comp: IComponent
    {
        /// <summary>Amount of time this instance has spent in simulation, since its creation, in seconds.
        /// Value may be changed by others (e.g. script, modifier).</summary>
        public double SimTime = 0;

        /// <summary>Delta time of the last Update() simulation step performed</summary>
        public double Dt = 0;

        /// <summary>Children components of this component, or null if none (yet)</summary>
        public List<Comp> Children = null;

        /// <summary>The parent component of this one, or null if none. Changing parent is done automatically
        /// via AddChild()</summary>
        public Comp Parent
        {
            get { return _parent; }
        }

        private Comp _parent = null;

        /// <summary>Called by Systems, to conveniently update any of the Comp members that need updating each cycle.</summary>
        /// <param name="dt">Time delta in seconds for current Update round</param>
        public void UpdateComp(double dt)
        {
            Dt = dt;
            SimTime += dt;                
        }

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
