using System;
using System.Text;
using System.Diagnostics;
using TTengine.Core;


namespace TTengine.Util
{
    /**
     * able to launch an external process and monitor it
     */
    public class Launchlet: Gamelet
    {
        public string FilePath = "";
        public Process Proc = null;
        public bool IsFailed = false;
        public bool IsStarted = false;
        public bool IsDone = false;

        //System.Diagnostics.Process.Exited ev;

        public Launchlet(string filepath): base()
        {
            this.FilePath = filepath;
        }

        protected void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            if (!IsStarted && !IsFailed)
            {
                try
                {
                    Proc = System.Diagnostics.Process.Start(FilePath);
                    Proc.Exited += new EventHandler(processExitedEvent);
                    Proc.EnableRaisingEvents = true;
                    IsStarted = true;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    IsFailed = true;
                }
                catch (System.ObjectDisposedException)
                {
                    IsFailed = true;
                }
                catch (System.IO.FileNotFoundException)
                {
                    IsFailed = true;
                }
            }

        }

        private void processExitedEvent(object sender, System.EventArgs e)
        {
            IsDone = true;
        }
    }
}
