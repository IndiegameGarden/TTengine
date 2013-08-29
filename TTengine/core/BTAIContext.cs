// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Artemis;
using Artemis.Interface;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// A context object that is passed to TreeNodes in the TreeSharp based
    /// BTAISystem. Contains references to objects and information useful
    /// for the AI.
    /// </summary>
    public class BTAIContext
    {
        /// <summary>
        /// The Entity being updated
        /// </summary>
        public Entity Entity;

        /// <summary>The component in this Entity, triggering the AI execution</summary>
        /// 
        public BTAIComp Comp;
        
        /// <summary>
        /// A globally kept simulation time value in seconds, 0f is start of simulation
        /// </summary>
        public double SimTime = 0.0;
        
        /// <summary>
        /// Delta t, the simulation time passed since last Update() in seconds.
        /// Equal to Delta time from the EntityWorld.
        /// </summary>
        public double Dt = 0.0;

        /// <summary>
        /// create with null or default values
        /// </summary>
        public BTAIContext()
        {
        }

        /// <summary>
        /// Copy all fields from another instance to the current one. 
        /// (e.g. for re-use of object, avoiding new object creation)
        /// </summary>
        public void CopyFrom(BTAIContext other)
        {
            SimTime = other.SimTime;
            Dt = other.Dt;
            Entity = other.Entity;
            Comp = other.Comp;
        }

    }
}
