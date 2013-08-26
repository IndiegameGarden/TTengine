using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis.Interface;
using TTengine.Core;

namespace TTengine.Comps
{
    /// <summary>
    /// AI component that records which Behaviors are enabled for the Entity
    /// </summary>
    public class AIComp: IComponent
    {
        public AIComp()
        {
        }

        /// <summary>Ordered list of Behaviors, highest-priority one first</summary>
        public List<Behavior> Behaviors = new List<Behavior>();
    }
}
