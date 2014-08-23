using System;

namespace TTengine.Modifiers
{
    /// <summary>
    /// IFunction computes an output value from an input
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Compute the function out value from input inp
        /// </summary>
        /// <param name="inp">function input value</param>
        /// <returns>function result value</returns>
        double Value(double inp);
    }
}
