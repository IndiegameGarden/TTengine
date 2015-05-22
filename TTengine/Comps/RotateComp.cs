using System;
using TTengine.Core;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Enables rotation of entities
    /// </summary>
    public class RotateComp: Comp
    {
        public RotateComp()
        {
        }

        /// <summary>Rotation angle in radians</summary>
        public double Rotate = 0;

        /// <summary>
        /// Rotation speed i.e. change of rotation, in radians/sec. 0 means no change.
        /// </summary>
        public double RotateSpeed = 0;

        /// <summary>
        /// Get the absolute value of rotation, also based on parent's rotation.
        /// </summary>
        public double RotateAbs
        {
            get
            {
                if (Parent == null)
                    return Rotate;
                else
                    return (Parent as RotateComp).RotateAbs;
            }
        }

    }
}
