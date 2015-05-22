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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.ScriptUpdateSystem)]
    public class ScriptSystemUpdate : EntityComponentProcessingSystem<ScriptComp>
    {
        ScriptContext ctx = new ScriptContext(); // single object re-used in all OnUpdate(ctx) calls
        double dt = 0;

        protected override void Begin()
        {
            dt = TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
            ctx.Dt = dt;
        }

        public override void Process(Entity entity, ScriptComp sc)
        {
            ctx.Entity = entity;
            sc.SimTime += dt;
            ctx.SimTime = sc.SimTime;
            foreach(IScript script in sc.Scripts)
                script.OnUpdate(ctx);
        }

    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScriptDrawSystem)]
    public class ScriptSystemDraw : EntityComponentProcessingSystem<ScriptComp>
    {
        ScriptContext ctx = new ScriptContext();

        protected override void Begin()
        {
            ctx.Dt = TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity, ScriptComp sc)
        {
            ctx.Entity = entity;
            ctx.SimTime = sc.SimTime;
            //foreach (var script in sc.Scripts) // FIXME
            //    script.OnDraw(ctx);

        }

    }
}
