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
        /// Counting period in seconds
        /// </summary>
        public double CountPeriod = 1.0;

        /// <summary>
        /// Time accumulated value over period, resets if reaching CountPeriod
        /// </summary>
        public double Time = 0.0;

        /// <summary>
        /// Time accumulative total, resets on Reset
        /// </summary>
        public double TimeTotal = 0.0;

        public bool IsRunning = false;

        protected DateTime TimeStart;
        protected int currentCount = 0;
        protected DateTime TimePrevUpdate;
        protected bool isFirstUpdate = true;

        public CountingTimer()
        {
        }

        public void Start()
        {
            TimeStart = FastDateTime.Now;
            TimePrevUpdate = TimeStart;
            IsRunning = true;
            isFirstUpdate = true;
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
        }

        public void Update()
        {
            if (!IsRunning) return;

            DateTime tNow = FastDateTime.Now;
            if (isFirstUpdate)
            {
                TimePrevUpdate = tNow;
                isFirstUpdate = false;
            }
            double dt = (tNow - TimePrevUpdate).TotalSeconds;
            TimePrevUpdate = tNow;
            Time += dt;
            TimeTotal += dt;
            if (Time > CountPeriod)
            {
                Time -= CountPeriod;
                Count = currentCount;
                currentCount = 0;
            }
        }

        public void CountUp()
        {
            currentCount++;
            CountTotal++;
        }

    }
}
