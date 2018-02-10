// (c) 2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System.Collections.Generic;
using System.Threading;

using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using TTengine.Comps;

namespace TTengine.Systems
{
    /// <summary>Queue System that executes script-jobs in a background thread.</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.BuilderSystem)]
    public class ScriptThreadedSystem : QueueSystemProcessingThreadSafe<ScriptJob>
    {
        BackgroundBuilder bgBuilder = null;

        public override void LoadContent()
        {
            //SetQueueProcessingLimit(1, typeof(ScriptThreadedSystem));
            bgBuilder = new BackgroundBuilder();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            this.IsEnabled = false;
            if (bgBuilder != null)
                bgBuilder.Stop();
            bgBuilder = null;
            base.UnloadContent();
        }

        public static void AddToQueue(ScriptJob script)
        {
            AddToQueue(script,typeof(ScriptThreadedSystem));
        }

        public override void Process(ScriptJob script)
        {
            bgBuilder.AddJob(script); // to separate thread for the background processing
        }
    }

    /// <summary>
    /// The threaded script execution engine that works in a background thread.
    /// </summary>
    class BackgroundBuilder
    {
        static Queue<ScriptJob> jobQ = new Queue<ScriptJob>();
        static Thread thread = null;
        static bool isRunning = true;
        int checkIntervalMs = 0;

        /// <summary>
        /// Create the BackgroundBuilder and start its thread
        /// </summary>
        /// <param name="checkIntervalMs">number of milliseconds to wait in between queue checks in case queue was found empty.</param>
        public BackgroundBuilder(int checkIntervalMs = 10)
        {
            this.checkIntervalMs = checkIntervalMs;
            if (thread == null)
                StartSystemThread();
        }

        protected void StartSystemThread()
        {
            thread = new Thread(new ThreadStart(RunSystemMainloop));
            thread.Name = "BackgroundBuilder";
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            thread.Interrupt();
            thread.Join();
        }

        public bool IsBusy()
        {
            lock (jobQ)
            {
                return jobQ.Count > 0;
            }
        }

        /// <summary>
        /// Add a new building job to the queue of the builder
        /// </summary>
        /// <param name="job">job to enqueue</param>
        public void AddJob(ScriptJob job)
        {
            lock (jobQ)
            {
                jobQ.Enqueue(job);
            }
        }

        /// <summary>
        /// Main loop which is run in separate builder-thread.
        /// </summary>
        protected void RunSystemMainloop()
        {
            try
            {
                while (isRunning)
                {
                    ScriptJob job = null;
                    lock (jobQ)
                    {
                        if (jobQ.Count > 0)
                        {
                            job = jobQ.Dequeue();
                        }
                    }
                    if (job != null)
                    {
                        // do the job - execute
                        job.Script(job.Entity.C<ScriptComp>());
                    }
                    else
                    {
                        // no job queued - sleep and then check again
                        Thread.Sleep(checkIntervalMs);
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                ;
            }
        }
    }

}
