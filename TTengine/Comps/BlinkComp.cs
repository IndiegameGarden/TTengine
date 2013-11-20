using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTengine.Core;

namespace TTengine.Comps
{
    public class BlinkComp : Comp
    {
        
        public BlinkComp(double timePeriod, double dutyCycle)
        {
            this.TimePeriod = timePeriod;
            this.DutyCycle = dutyCycle;
        }

        /// <summary>
        /// the length of a single blink period
        /// </summary>
        public double TimePeriod
        {
            get
            {
                return timePeriod;
            }
            set
            {
                timePeriod = value;
                timeOn = dutyCycle * timePeriod;
            }
        }

        /// <summary>
        /// the fraction 0...1 of time that the blinking thing is visible
        /// </summary>
        public double DutyCycle
        {
            get
            {
                return dutyCycle;
            }
            set
            {
                dutyCycle = value;
                timeOn = dutyCycle * TimePeriod;
            }
        }

        /// <summary>
        /// the time (in seconds) that is spent in the 'on'/visible state, which is TimePeriod * DutyCycle.
        /// </summary>
        public double TimeOn
        {
            get
            {
                return timeOn;
            }
        }

        /// <summary>
        /// whether the current blinking behavior sets the Entity in a visible state (true), or invisible (false).
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
        }

        protected double timeOn, dutyCycle, timePeriod;
        protected bool isVisible = true;

    }

}
