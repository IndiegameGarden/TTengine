
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// Function to compute an output double value from an input double value. 
    /// </summary>
    public abstract class Function: IFunction
    {
        public abstract double Value(double inp);

        public double Value(ScriptComp sc)
        {
            return this.Value(sc.SimTime);
        }
    }
}
