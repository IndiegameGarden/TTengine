using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// a basic Gamelet timing component including start-time definition and duration
    /// </summary>
    public class TimingComp: TTObject
    {
        public const float DURATION_INFINITY = -1f;

        public float StartTime = 0f;

        public float Duration = DURATION_INFINITY;

        public TimingComp(float startTime, float duration)
        {
            StartTime = startTime;
            Duration = duration;
        }

        public TimingComp(float startTime)
            : this(startTime, DURATION_INFINITY)
        {
            //
        }

        /// <summary>
        /// Add timed behavior to a Gamelet
        /// </summary>
        /// <param name="g">Gamelet to add a Timing component to </param>
        /// <param name="startTime">time at which Gamelet will be activated</param>
        /// <param name="duration">time duration of being Active, after which Gamelet is auto-deleted</param>
        public static void AddTiming(Gamelet g, float startTime, float duration)
        {
            if (g.Timing != null)
            {
                g.Remove(g.Timing);
            }
            g.Timing = new TimingComp(startTime, duration);
            g.Add(g.Timing);
        }

        public static void AddTiming(Gamelet g, float startTime)
        {
            AddTiming(g, startTime, DURATION_INFINITY);
        }

        /// <summary>
        /// adds Timing component to Gamelet without setting its parameters yet, i.e. an always-on behavior
        /// </summary>
        /// <param name="g"></param>
        public static void AddTiming(Gamelet g)
        {
            AddTiming(g, 0f);
        }

        public override void OnInit()
        {
            //
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            if (StartTime >= 0f)
            {
                if (!Parent.Active && Parent.SimTime >= StartTime )
                {
                    Parent.Active = true;
                }
            }

            if (Duration >= 0f)
            {
                if (Parent.Active && (Parent.SimTime - StartTime) > Duration )
                {
                    Parent.Active = false;
                }
            }
        }

        public override void OnNewParent()
        {
            Parent.Active = false; // allow timed activation later in OnUpdate()
        }
    }
}
