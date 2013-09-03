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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    public class ScriptSystemUpdate : EntityComponentProcessingSystem<ScriptComp>
    {
        static ScriptContext ctx = new ScriptContext();

        public override void Process(Entity entity, ScriptComp sc)
        {
            if (!sc.IsActive) return;
            sc.UpdateComp(this);
            ctx.ScriptComp = sc;
            ctx.Entity = entity;
            sc.Script.OnUpdate(ctx);
        }

    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 1)]
    public class ScriptSystemDraw : EntityComponentProcessingSystem<ScriptComp>
    {
        static ScriptContext ctx = new ScriptContext();

        public override void Process(Entity entity, ScriptComp sc)
        {
            if (!sc.IsActive) return;
            ctx.ScriptComp = sc;
            ctx.Entity = entity;
            sc.Script.OnDraw(ctx);
        }

    }
}
