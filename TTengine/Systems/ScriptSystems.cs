using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using TTengine.Comps;

using Artemis;
using Artemis.Manager;
using Artemis.Attributes;
using Artemis.System;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.ScriptSystemUpdate)]
    public class ScriptSystemUpdate : EntityComponentProcessingSystem<ScriptComp>
    {
        ScriptContext ctx = new ScriptContext();
        double dt = 0;

        protected override void Begin()
        {
            dt = TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
            ctx.Dt = dt;
        }

        public override void Process(Entity entity, ScriptComp sc)
        {
            sc.UpdateComp(dt);
            ctx.Entity = entity;
            ctx.SimTime = sc.SimTime;
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
