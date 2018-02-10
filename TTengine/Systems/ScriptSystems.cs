// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

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
        public override void Process(Entity entity, ScriptComp sc)
        {            
            sc.SimTime += Dt;
            sc.Dt = Dt;
            foreach (var script in sc.Scripts)
                script.OnUpdate(sc);
        }

    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.ScriptSystemDraw)]
    public class ScriptSystemDraw : EntityComponentProcessingSystem<ScriptComp>
    {
        public override void Process(Entity entity, ScriptComp sc)
        {
            foreach (var script in sc.Scripts)
                script.OnDraw(sc);
        }
    }
}
