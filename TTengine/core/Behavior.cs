using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis;
using Artemis.Interface;

namespace TTengine.Core
{
    /// <summary>An atomic piece of AI script defining a specific aspect of Entity behavior</summary>
    public class Behavior: IComponent
    {
        public Behavior()
        {
        }

        /// <summary>Flag indicating whether the Behavior is 'active', i.e. 'triggers'. May change dynamically 
        /// depending on conditions calculated by the specific Behavior during OnUpdate().</summary>
        public virtual bool IsActive {get; set;}

        /// <summary>called every update cycle of the BTAISystem</summary>
        /// <param name="p">Informative update parameters that may be used by the Behavior</param>
        public virtual void OnUpdate(UpdateParams p)
        {
        }

        /// <summary>Called when the BTAISystem selects this Behavior for execution.</summary>
        public virtual void OnExecute(UpdateParams p)
        {
        }

    }
}
