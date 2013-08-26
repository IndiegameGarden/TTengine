using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Behaviors
{
    public class BlinkBehavior : Behavior
    {
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

        public BlinkBehavior(double timePeriod, double dutyCycle)
        {
            this.TimePeriod = timePeriod;
            DutyCycle = dutyCycle;
            IsActive = true;
        }

        public override void OnUpdate(UpdateParams p)
        {
            double t = p.SimTime % timePeriod;
            if (t <= timeOn)
                isVisible = true;
            else
                isVisible = false;
        }

        public override void OnExecute(UpdateParams p)
        {
            p.Entity.GetComponent<DrawComp>().IsVisible = isVisible;
        }

    }

}
