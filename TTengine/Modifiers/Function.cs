using System;

namespace TTengine.Modifiers
{
    /// <summary>
    /// Function to compute an output value from an input. Functions can be
    /// concatenated by passing an IFunction in the constructor.
    /// </summary>
    public class Function: IFunction
    {
        protected IFunction innerFunction = null;

        /// <summary>
        /// Create new Function
        /// </summary>
        public Function()
        {
        }

        /// <summary>
        /// Create new Function that uses the result of another IFunction as its input value.
        /// </summary>
        /// <param name="innerFunction"></param>
        public Function(IFunction innerFunction)
        {
            this.innerFunction = innerFunction;
        }

        /// <summary>
        /// The value computation, implements IFunction. Can be overridden as long as the
        /// child class uses the line 'inp = base.Value(inp);' at the start to have the
        /// concatenation done properly.
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        public virtual double Value(double inp)
        {
            double val;
            if (innerFunction != null)
                val = innerFunction.Value(inp);
            else
                val = inp;
            return val;
        }
    }
}
