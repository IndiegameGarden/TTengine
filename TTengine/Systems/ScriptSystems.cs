using System;
using System.Collections.Generic;

using System.Text;
using TTengine.Core;
using TTengine.Comps;

using Artemis;
using Artemis.Manager;
using Artemis.Attributes;
using Artemis.System;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.ScriptSystem)]
    public class ScriptSystemUpdate : EntityComponentProcessingSystem<ScriptComp>
    {
        ScriptContext ctx = new ScriptContext(); // single object re-used in all OnUpdate(ctx) calls

        protected override void Begin()
        {
            ctx.Dt = this.Dt;
        }

        public override void Process(Entity entity, ScriptComp sc)
        {
            ctx.Entity = entity;
            sc.SimTime += Dt;
            ctx.SimTime = sc.SimTime;
            //ctx.Dt = set already in the Begin() method above
            foreach(IScript script in sc.Scripts)
                script.OnUpdate(ctx);
        }

    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScriptSystemDraw)]
    public class ScriptSystemDraw : EntityComponentProcessingSystem<ScriptComp>
    {
        ScriptContext ctx = new ScriptContext();

        protected override void Begin()
        {
            ctx.Dt = this.Dt;
        }

        public override void Process(Entity entity, ScriptComp sc)
        {
            ctx.Entity = entity;
            ctx.SimTime = sc.SimTime;
            foreach (var script in sc.Scripts)
            {
                if (script is IScriptDraw)
                    (script as IScriptDraw).OnDraw(ctx);
            }

        }
    }
}
