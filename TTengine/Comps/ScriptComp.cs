using System;
using System.Collections.Generic;

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
        /// <summary>Amount of time active in simulation of the parent ScriptComp, in seconds.
        /// Value may be changed by others (e.g. script, modifier).</summary>
        public double SimTime = 0;

        /// <summary>Delta time of the last Update() simulation step performed</summary>
        public double Dt = 0;

        /// <summary>
        /// The Entity that the script is attached to
        /// </summary>
        public Entity Entity;
    }

    /// <summary>
    /// Interface that a script object must implement
    /// </summary>
    public interface IScript
    {
        void OnUpdate(ScriptContext context);
    }

    /// <summary>
    /// Interface that a script object must implement
    /// </summary>
    public interface IScriptDraw: IScript
    {
        void OnDraw(ScriptContext context);
    }

    /// <summary>
    /// The Comp that enables scripting for your Entity with one or more ordered scripts.
    /// </summary>
    public class ScriptComp: Comp
    {
        /// <summary>Simulation time counter that is used by scripts via ScriptContext</summary>
        public double SimTime;

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
        /// <param name="script">script to Add initially</param>
        public ScriptComp(IScript script)
        {
            Add(script);
        }

        /// <summary>
        /// Add a new IScript script to execute to this component.
        /// </summary>
        /// <param name="script">The IScript to add and execute in next updates.</param>
        public void Add(IScript script)
        {
            this.Scripts.Add(script);
        }

    }
}
