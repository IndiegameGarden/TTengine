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
    /// The Comp that enables scripting for your Entity
    /// </summary>
    public class ScriptComp: Comp
    {
        /// <summary>
        /// The script that is called every update/draw cycle
        /// </summary>
        public IScript Script;

        public ScriptComp(IScript script)
        {
            this.Script = script;
        }

    }
}
