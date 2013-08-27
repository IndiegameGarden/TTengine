using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis.Interface;

namespace TTengine.Core
{
    /// <summary>optional base class for components</summary>
    public class Comp: IComponent
    {
        /// <summary>Indicate to the processing system whether this component is currently active, or not</summary>
        public bool IsActive = true;
    }
}
