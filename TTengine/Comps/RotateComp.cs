using System;
using TTengine.Core;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Enables rotation of entities
    /// </summary>
    public class RotateComp: IComponent
    {
        public RotateComp()
        {
        }

        /// <summary>Rotation angle in radians</summary>
        public double Rotate = 0;

        /// <summary>
        /// Rotation speed i.e. change of rotation, in radians/sec
        /// </summary>
        public double RotateSpeed = 0;
    }
}
