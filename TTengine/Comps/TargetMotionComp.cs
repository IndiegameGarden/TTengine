// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Artemis.Interface;
using TTengine.Core;

namespace TTengine.Comps
{
    /// <summary>
    /// Component for simple physics-based motion 
    /// (velocity, scale, rotation, zoom, etc.)
    /// FIXME not used yet
    /// </summary>
    public class TargetMotionComp : IComponent
    {
        public TargetMotionComp()
        {
        }

        public Vector3 Target
        {
            get
            {
                return targetPos;
            }
            set
            {
                targetPos = value;
                isTargetSet = true;
            }
        }

        /// <summary>
        /// velocity of moving towards target TargetPos. Setting modifies Velocity.
        /// </summary>
        public double TargetVelocity
        {
            get;
            set;
        }

        internal Vector3 targetPos = Vector3.Zero;
        internal bool isTargetSet = false;

    }
}
