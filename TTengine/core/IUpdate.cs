using System;

namespace TTengine.Core
{
    interface IUpdate
    {
        public void OnUpdate(double dt, double simTime);
    }
}
