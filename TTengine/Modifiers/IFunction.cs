
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// Function that computes an output double value from an input double value
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Compute the function out value from input inp
        /// </summary>
        /// <param name="inp">function input value</param>
        /// <returns>function result value</returns>
        double Value(double inp);

        /// <summary>
        /// Compute this.Value(sc.SimTime), a shorthand for this often-used construct.
        /// </summary>
        /// <param name="sc">ScriptComp from which SimTime field is used.</param>
        /// <returns>this.Value(sc.SimTime)</returns>
        double Value(ScriptComp sc);
    }
}
