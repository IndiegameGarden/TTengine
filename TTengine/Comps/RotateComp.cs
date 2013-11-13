using System;
using TTengine.Core;

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
    }
}
