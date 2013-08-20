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
        /// speed for scaling towards ScaleTarget
        /// </summary>
        public double ScaleSpeed = 0;

        /*
        protected void ScaleToTarget(float targetScale, float spd, float spdMin)
        {
            if (spd > 0)
            {
                if (Scale < targetScale)
                {
                    Scale += spdMin + spd * (targetScale - Scale); //*= 1.01f;
                    if (Scale > targetScale)
                    {
                        Scale = targetScale;
                    }
                }
                else if (Scale > targetScale)
                {
                    Scale += -spdMin + spd * (targetScale - Scale); //*= 1.01f;
                    if (Scale < targetScale)
                    {
                        Scale = targetScale;
                    }
                }
            }
        }
        */
    }
}
