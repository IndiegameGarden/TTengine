// (c) 2010-2015 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

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
        /// <summary>The Entity being updated</summary>
        public Entity Entity;

        /// <summary>The BTAIComp of Entity, which triggers BTAI processing</summary>
        public BTAIComp BTComp;

        /// <summary>Globally kept simulation time value in seconds, 0 is start time of BTAISystem</summary>
        public double SimTime = 0.0;
        
        /// <summary>Delta t, the simulation time passed since last Update() in seconds. Equal to Delta time from the EntityWorld.</summary>
        public double Dt = 0.0;

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
            BTComp = other.BTComp;
            Dt = other.Dt;
            Entity = other.Entity;
        }

    }
}
