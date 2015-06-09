using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using TTengine.Comps;
using Microsoft.Xna.Framework;

namespace PXengine.Comps
{
    public class ColorCycleComp: Comp
    {
        public double SimTime = 0.0;
        public double timePeriod;
        public double timePeriodR, timePeriodG, timePeriodB, timePeriodA;
        public Color minColor;
        public Color maxColor;

        public ColorCycleComp(float timePeriod)
        {
            this.timePeriod = timePeriod;
            timePeriodR = timePeriod;
            timePeriodG = timePeriod;
            timePeriodB = timePeriod;
            timePeriodA = timePeriod;
        }

    }
}
