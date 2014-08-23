using System;
using System.Collections.Generic;

using Artemis;
using Artemis.Interface;
using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// A script object that first computes a function and then calls a delegate (piece of code) to use
    /// that function value. The script usually will modify one or more parameters of Components.
    /// </summary>
    public class ModifierScript: IScript
    {
        /// <summary>
        /// The script code
        /// </summary>
        public ScriptDelegate ScriptCode;

        /// <summary>
        /// The Function that is computed and whose value is passed in ScriptContext.FunctionValue to the
        /// ScriptCode.
        /// </summary>
        public IFunction Function;

        /// <summary>
        /// Create new ModifierScript
        /// </summary>
        /// <param name="code"></param>
        /// <param name="function">if null is passed, the unity function f(x)=x is applied as Function</param>
        public ModifierScript(ScriptDelegate code, IFunction function)
        {
            this.ScriptCode = code;
            this.Function = function;
        }

        /// <summary>
        /// Implements IScript
        /// </summary>
        /// <param name="ctx"></param>
        public void OnUpdate(ScriptContext ctx)
        {
            if (Function == null)
                ctx.FunctionValue = ctx.SimTime;
            else
                ctx.FunctionValue = Function.Value(ctx.SimTime);
            ScriptCode(ctx);
        }

    }
}
