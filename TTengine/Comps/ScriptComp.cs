using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using Artemis;

namespace TTengine.Comps
{
    /// <summary>
    /// Method signature (Delegate) for scripts
    /// </summary>
    /// <param name="ctx">script context supplied during script execution</param>
    public delegate void ScriptDelegate(ScriptContext ctx);

    /// <summary>
    /// The context object passed to scripts when run
    /// </summary>
    public class ScriptContext
    {
        public ScriptComp ScriptComp;
        public Entity Entity;
    }

    /// <summary>
    /// Interface that a script object must implement
    /// </summary>
    public interface IScript
    {
        void OnUpdate(ScriptContext context);
        void OnDraw(ScriptContext context);
    }

    /// <summary>
    /// Basic script object that can run code from a Delegate
    /// </summary>
    public class Script: IScript
    {
        protected ScriptDelegate scriptFunction ;
        protected ScriptComp sc;

        public Script(ScriptDelegate scriptFunction, ScriptComp sc)
        {
            this.scriptFunction = scriptFunction;
            this.sc = sc;
        }

        public void OnUpdate(ScriptContext ctx)
        {
            scriptFunction(ctx);
        }

        public void OnDraw(ScriptContext ctx)
        {
            // nothing
        }

    }

    /// <summary>
    /// The Comp that enables scripting for your Entity with one or more ordered scripts.
    /// </summary>
    public class ScriptComp: Comp
    {
        /// <summary>
        /// The scripts that are called every update/draw cycle
        /// </summary>
        public List<IScript> Scripts = new List<IScript>();

        /// <summary>
        /// Create new ScriptComp without any scripts yet
        /// </summary>
        public ScriptComp()
        {        
        }

        /// <summary>
        /// Create new ScriptComp with a single script already added
        /// </summary>
        /// <param name="script">script to Add</param>
        public ScriptComp(IScript script)
        {
            Add(script);
        }

        public void Add(IScript script)
        {
            this.Scripts.Add(script);
        }

        /// <summary>
        /// Add new script based on a function. This makes script creation quick - no need to create
        /// a class for this, just a single method.
        /// </summary>
        /// <param name="scriptFunction">method/function (delegate) to add as script</param>
        /// <returns>The IScript object created from the function/delegate</returns>
        public IScript Add(ScriptDelegate scriptFunction)
        {
            var script = new Script(scriptFunction, this);
            this.Add(script);
            return script;
        }
    }
}
