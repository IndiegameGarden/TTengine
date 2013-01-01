// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

namespace TTengine.Core
{
    /**
     * a base class for an IState implementation
     */
    public class State: IState
    {
        /// <summary>
        /// simulation time spent in this state until now since entry
        /// </summary>
        public float SimTime = 0f;

        public virtual void OnEntry(Gamelet g)
        {
            SimTime = 0f;
        }

        public virtual void OnExit(Gamelet g)
        {            
        }

        public virtual void OnUpdate(Gamelet g, ref UpdateParams p)
        {
            SimTime += p.Dt;
        }

        public virtual void OnDraw(Gamelet g)
        {
        }

    }
}
