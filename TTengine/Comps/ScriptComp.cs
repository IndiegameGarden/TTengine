using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using Artemis;

namespace TTengine.Comps
{
    public class ScriptContext
    {
        public ScriptComp ScriptComp;
        public Entity Entity;
    }

    public interface IScript
    {
        void OnUpdate(ScriptContext context);
        void OnDraw(ScriptContext context);
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

        public ScriptComp()
        {        
        }

        public ScriptComp(IScript script)
        {
            Add(script);
        }

        public void Add(IScript script)
        {
            this.Scripts.Add(script);
        }

    }
}
