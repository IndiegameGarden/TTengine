using System;

namespace TTengine.Core
{
    /// <summary>
    /// Interface for any updatable object, e.g. a script or modifier
    /// </summary>
    public interface IUpdate
    {
        void OnUpdate(double dt, double simTime);
    }
}
