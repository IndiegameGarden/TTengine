using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis.Interface;
using Artemis.System;

namespace TTengine.Core
{
    /// <summary>optional base class for components</summary>
    public class Comp: IComponent
    {
        /// <summary>Indicate to the processing system whether this component is currently active, or not</summary>
        public bool IsActive = true;

        /// <summary>Amount of time this instance has spent in simulation, since its creation, in seconds</summary>
        public double SimTime = 0;

        /// <summary>Delta time of the last simulation step performed</summary>
        public double Dt = 0;

        /// <summary>Called by TTengine Systems, to conveniently update any of the Comp members that need updating each cycle.</summary>
        /// <param name="sys"></param>
        internal void UpdateComp(EntitySystem sys)
        {
            // TODO FIXME optimize with the fixed ticks-to-seconds scale factor? - get readymade from another location/class/ttgame?
            Dt = TimeSpan.FromTicks(sys.EntityWorld.Delta).TotalSeconds;
            SimTime += Dt;                
        }
    }
}
