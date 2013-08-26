// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Artemis;
using Artemis.Interface;

namespace TTengine.Core
{
    public class UpdateParams
    {
        /// <summary>
        /// The Entity being updated
        /// </summary>
        public Entity Entity;

        /// <summary>The component triggering the OnUpdate</summary>
        /// 
        public IComponent Comp;
        
        /// <summary>
        /// A globally kept simulation time value in seconds, 0f is start of simulation
        /// </summary>
        public double SimTime = 0.0;
        
        /// <summary>
        /// Delta t, the simulation time passed since last Update() in seconds
        /// </summary>
        public double Dt = 0.0;

        /// <summary>
        /// create all params with null or default values
        /// </summary>
        public UpdateParams()
        {
        }

        /// <summary>
        /// Copy all fields from an 'other' params to the current one. 
        /// (Useful for re-initializing avoiding new allObj creation)
        /// </summary>
        public void CopyFrom(UpdateParams other)
        {
            SimTime = other.SimTime;
            Dt = other.Dt;
            Entity = other.Entity;
            Comp = other.Comp;
        }

    }
}
