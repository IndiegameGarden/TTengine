// (c) 2010-2015 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
using Microsoft.Xna.Framework;
using Artemis.Interface;
using TTengine.Core;

namespace TTengine.Comps
{
    /// <summary>
    /// Component for scale modification
    /// </summary>
    public class ScaleComp : Comp
    {
        public ScaleComp():
            this(1)
        {
        }

        public ScaleComp(double scale)
        {
            this.Scale = scale;
        }

        /// <summary>
        /// the relative size scaling factor, 1.0 being normal scale
        /// </summary>
        public double Scale = 1;

        /// <summary>
        /// set a target for Scale value
        /// </summary>
        public double ScaleTarget = 1;

        /// <summary>
        /// speed for changing Scale towards ScaleTarget (speed can be 0: no change)
        /// </summary>
        public double ScaleSpeed = 0;

        /// <summary>
        /// The absolute scale, obtained by multiplying this Entity's scale with its
        /// parent absolute scale.
        /// </summary>
        public double ScaleAbs
        {
            get
            {
                if (Parent == null)
                    return Scale;
                else
                    return Scale * (Parent as ScaleComp).ScaleAbs;                
            }
        }

    }
}
