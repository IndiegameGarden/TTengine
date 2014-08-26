using System;
using Microsoft.Xna.Framework;
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// IVectorFunction computes a Vector3 output value from an input context
    /// </summary>
    public interface IVectorFunction
    {
        /// <summary>
        /// Compute the function out value from current script context
        /// </summary>
        /// <param name="ctx">Script context object</param>
        /// <returns>Vector function result value</returns>
        Vector3 Value(ScriptContext ctx);
    }
}
