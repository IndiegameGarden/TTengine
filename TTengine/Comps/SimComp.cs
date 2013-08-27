using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;

namespace TTengine.Comps
{
    /// <summary>
    /// Comp with simulation related parameters
    /// </summary>
    public class SimComp: Comp
    {
        /// <summary>Amount of time this component has spent in simulation, since its creation, in seconds</summary>
        public double SimTime = 0;
    }
}
