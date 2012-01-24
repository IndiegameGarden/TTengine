// (c) 2010-2012 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

namespace TTengine.Core
{
    /**
     * a base class for an IState implementation
     */
    public class State: IState
    {
        public virtual void OnEntry(Gamelet g)
        {
        }

        public virtual void OnExit(Gamelet g)
        {
            
        }

        public virtual void OnUpdate(Gamelet g)
        {
        }

    }
}
