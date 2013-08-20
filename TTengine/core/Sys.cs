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
    public class Sys: Comp
    {
        public Type CompType;

        protected List<Comp> compInstances = null;

        public Sys(Type compType)
        {
            this.CompType = compType;
            // get ref to the list which keeps all instances of this specific Comp class ('type').
            compInstances = Comp.CompsDict[compType];
        }

        public virtual void Update(ref UpdateParams p)
        {
            foreach (Comp c in compInstances)
            {

            }
        }

    }
}
