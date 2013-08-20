using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// a basic timing component including start-time definition and duration
    /// </summary>
    public class TimingComp: Comp
    {
        public TimingComp(double startTime, double duration)
        {
            Register(this);
            StartTime = startTime;
            Duration = duration;
        }

        public TimingComp(double startTime)
            : this(startTime, DURATION_INFINITY)
        {
            //
        }

        public const double DURATION_INFINITY = -1;

        public double StartTime = -1;

        public double Duration = DURATION_INFINITY;

        /// <summary>
        /// Add timed behavior to a Gamelet
        /// </summary>
        /// <param name="g">Gamelet to add a Timing component to </param>
        /// <param name="startTime">time at which Gamelet will be activated</param>
        /// <param name="duration">time duration of being Active, after which Gamelet is auto-deleted</param>
        public static void AddTo(Gamelet g, double startTime, double duration)
        {
            TimingComp tc = new TimingComp(startTime, duration);
            g.Add(tc);
        }

        public static void AddTo(Gamelet g, double startTime)
        {
            AddTo(g, startTime, DURATION_INFINITY);
        }

        /// <summary>
        /// adds Timing component to Gamelet without setting its parameters yet, i.e. an always-on behavior
        /// </summary>
        /// <param name="g"></param>
        public static void AddTo(Gamelet g)
        {
            AddTo(g, 0f);
        }

        /// <summary>
        /// FIXME: run by System
        /// </summary>
        /// <param name="p"></param>
        protected void OnUpdate(ref UpdateParams p)
        {
            if (StartTime >= 0)
            {
                if (!Parent.Active && Parent.SimTime >= StartTime )
                {
                    Parent.Active = true;
                }
            }

            if (Duration >= 0)
            {
                if (Parent.Active && (Parent.SimTime - StartTime) > Duration )
                {
                    Parent.Active = false;
                }
            }
        }

        public void OnNewParent(Gamelet oldParent)
        {
            Parent.Active = false; // allow timed activation by this component
        }
    }
}
