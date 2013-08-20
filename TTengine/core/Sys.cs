// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// Base class for all Systems in the Entity-Component System pattern.
    /// </summary>
    public abstract class Sys: Comp
    {
        public Type CompType;

        protected List<Comp> CompInstances = null;

        public Sys(Type compType)
        {
            this.CompType = compType;
            // get ref to the list which keeps all instances of this specific Comp class ('type').
            CompInstances = Comp.CompsDict[compType];
        }

        public abstract void UpdateComp(Comp c, UpdateParams p);

        public virtual void Update(UpdateParams p)
        {
            foreach (Comp c in CompInstances)
            {
                UpdateComp(c,p);
            }
        }

    }
}
