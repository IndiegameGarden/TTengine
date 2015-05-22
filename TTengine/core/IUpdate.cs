using System;

namespace TTengine.Core
{
    public interface IUpdate
    {
        void OnUpdate(double dt, double simTime);
    }
}
