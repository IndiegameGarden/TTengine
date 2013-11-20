﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis.Interface;
using Artemis.System;

namespace TTengine.Core
{
    /// <summary>optional base class for components that implement IComponent</summary>
    public abstract class Comp: IComponent
    {
        /// <summary>Amount of time this instance has spent in simulation, since its creation, in seconds</summary>
        public double SimTime = 0;

        /// <summary>Delta time of the last simulation step performed</summary>
        public double Dt = 0;

        /// <summary>Children components of this component, or null if none (yet)</summary>
        public List<Comp> Children = null;

        /// <summary>The parent component of this one, or null if none</summary>
        public Comp Parent = null;

        /// <summary>Called by TTengine Systems, to conveniently update any of the Comp members that need updating each cycle.</summary>
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
            child.Parent = this;
        }

    }
}
