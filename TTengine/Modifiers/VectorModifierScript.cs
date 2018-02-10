﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Artemis;
using Artemis.Interface;
using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// Method signature (Delegate) for modifier scripts
    /// </summary>
    /// <param name="ctx">script context supplied during script execution</param>
    /// <param name="functionValue">value of function that is passed to script code</param>
    public delegate void VectorScriptDelegate(ScriptComp ctx, Vector3 functionValue);

    /// <summary>
    /// A script object that first computes a function and then calls a delegate (piece of code) to use
    /// that function value. The script usually will modify one or more parameters of Components.
    /// </summary>
    public class VectorScript: IScript
    {
        /// <summary>
        /// The script code
        /// </summary>
        public VectorScriptDelegate ScriptCode;

        /// <summary>
        /// The Function that is computed and whose value is passed in ScriptComp.FunctionValue to the
        /// ScriptCode.
        /// </summary>
        public IVectorFunction Function;

        /// <summary>
        /// Create new ModifierScript
        /// </summary>
        /// <param name="code"></param>
        /// <param name="function">if null is passed, the unity function f(x)=x is applied as Function</param>
        public VectorScript(VectorScriptDelegate code, IVectorFunction function)
        {
            this.ScriptCode = code;
            this.Function = function;
        }

        /// <summary>
        /// Implements IScript
        /// </summary>
        /// <param name="ctx"></param>
        public void OnUpdate(ScriptComp ctx)
        {
            var v = Function.Value(ctx);
            ScriptCode(ctx,v);
        }

        public void OnDraw(ScriptComp ctx) {; }
    }
}
