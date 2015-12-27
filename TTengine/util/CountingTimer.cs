using System;
using Artemis.Utils;

namespace TTengine.Util
{
    public class CountingTimer
    {
        /// <summary>
        /// Count accumulative value over period CountPeriod, updated every CountPeriod
        /// </summary>
        public int Count = 0;

        /// <summary>
        /// Count accumulative total since last Reset
        /// </summary>
        public int CountTotal = 0;

        /// <summary>
        /// Counting period in seconds, of real time
        /// </summary>
        public double CountPeriod = 1.0;

        /// <summary>
        /// Time accumulated value over period, increases when timer IsRunning
        /// </summary>
        public double Time = 0.0;

        public double TimeReal = 0.0;

        /// <summary>
        /// Time accumulative total, resets on Reset, increases when timer IsRunning
        /// </summary>
        public double TimeTotal = 0.0;

        public double TimePerCount
        {
            get
            {
                return Time / Count;
            }
        }

        public double CountPerTime
        {
            get
            {
                return Count / Time;
            }

        }
        
        public bool IsRunning = false;

        protected int currentCount = 0;
        protected double currentTime = 0.0;
        protected DateTime TimePrevUpdate, TimePrevStartOrUpdate;

        public CountingTimer()
        {
            TimePrevUpdate = FastDateTime.Now;
            TimePrevStartOrUpdate = TimePrevUpdate;
        }

        public void Start()
        {
            IsRunning = true;
            TimePrevStartOrUpdate = FastDateTime.Now;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Reset()
        {
            Count = 0;
            CountTotal = 0;
            TimeTotal = 0.0;
            Time = 0.0;
            currentCount = 0;
            TimePrevUpdate = FastDateTime.Now;
            TimePrevStartOrUpdate = TimePrevUpdate;
        }

        public void Update()
        {
            if (!IsRunning) return;

            DateTime tNow = FastDateTime.Now;
            double dt1 = (tNow - TimePrevUpdate).TotalSeconds; // counts real elapsed time
            double dt2 = (tNow - TimePrevStartOrUpdate).TotalSeconds; // counts IsRunning active time only
            TimeReal += dt1;
            currentTime += dt2;
            TimeTotal += dt2;
            if (TimeReal > CountPeriod)
            {
                TimeReal -= CountPeriod;
                Count = currentCount;
                Time = currentTime;
                currentCount = 0;
                currentTime = 0.0;
            }

            // state for next round
            TimePrevUpdate = tNow;
            TimePrevStartOrUpdate = tNow;
        }

        public void CountUp()
        {
            currentCount++;
            CountTotal++;
        }

    }
}
