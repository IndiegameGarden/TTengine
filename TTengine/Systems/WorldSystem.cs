﻿
namespace TTengine.Systems
{
    using System;
    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;
    using TTengine.Core;
    using TTengine.Comps;

    /// <summary>The system for simulating multiple worlds</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.WorldSystem)]
    public class WorldSystem : EntityComponentProcessingSystem<WorldComp>
    {
        public override void Process(Entity entity, WorldComp worldComp)
        {
            if (worldComp.TimeWarp != 1.0)
                worldComp.World.Update(new TimeSpan((long)((double)EntityWorld.Delta * worldComp.TimeWarp)));
            else
                worldComp.World.Update(EntityWorld.DeltaTimeSpan);
        }
    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.WorldSystemDraw)]
    public class WorldRenderSystem : EntityComponentProcessingSystem<WorldComp>
    {
        public override void Process(Entity entity, WorldComp worldComp)
        {
            worldComp.World.Draw();						// render the World
        }
    }
}