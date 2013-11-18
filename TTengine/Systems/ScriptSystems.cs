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
        }

        public override void Process(Entity entity, ScriptComp sc)
        {
            if (!sc.IsActive) return;
            sc.UpdateComp(dt);
            ctx.ScriptComp = sc;
            ctx.Entity = entity;
            foreach(var script in sc.Scripts)
                script.OnUpdate(ctx);
        }

    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScriptSystemDraw)]
    public class ScriptSystemDraw : EntityComponentProcessingSystem<ScriptComp>
    {
        static ScriptContext ctx = new ScriptContext();

        public override void Process(Entity entity, ScriptComp sc)
        {
            if (!sc.IsActive) return;
            ctx.ScriptComp = sc;
            ctx.Entity = entity;
            foreach (var script in sc.Scripts)
                script.OnDraw(ctx);
        }

    }
}
