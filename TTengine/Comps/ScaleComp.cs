// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /// <summary>
    /// Component for scale modification
    /// </summary>
    public class ScaleComp : Comp
    {
        public ScaleComp()
        {
            Register(this);
        }

        public double Scale = 1;

        public double ScaleAbs = 1;
        
        /// <summary>
        /// set target for Scale value
        /// </summary>
        public double ScaleTarget = 1;

        /// <summary>
        /// speed for scaling towards ScaleTarget (can be 0)
        /// </summary>
        public double ScaleSpeed = 0;

    }
}
