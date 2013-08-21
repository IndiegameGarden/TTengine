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
        //public uint CompTypeID;
        public bool DoesFollowEntityHierarchy;
        protected List<Comp> CompInstances = null;

        protected Sys(Type compType, bool doesFollowEntityHierarchy)
        {
            this.CompType = compType;
            //this.CompTypeID = compTypeID;
            this.DoesFollowEntityHierarchy = doesFollowEntityHierarchy;
            // get ref to the list which keeps all instances of this specific Comp class ('type').
            CompInstances = Comp.CompsDict[compType];
        }

        public abstract void UpdateComp(Comp c, UpdateParams p);

        public virtual void Update(UpdateParams p, Gamelet root)
        {
            if (!DoesFollowEntityHierarchy)
            {
                foreach (Comp c in CompInstances)
                {
                    UpdateComp(c, p);
                }
            }
            else // does follow entity hierarchy
            {
                UpdateGamelet(p, root);
            }
        }

        /// <summary>
        /// recursively called function to process Comps in hierarchical order.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="g"></param>
        private void UpdateGamelet(UpdateParams p, Gamelet g)
        {
            // Process Comps in the Gamelet
            foreach (Comp c in g.Comps)
            {
                if (c.GetType() == CompType)  // TODO: future optimization for faster check e.g. uint CompType based. Benchmark to be done.
                    UpdateComp(c, p);
            }

            // Recursively process child Gamelets and their components
            foreach (Gamelet cg in g.Children)
            {
                UpdateGamelet(p, cg);
            }
        }

    }
}
