namespace TTengine.Systems
{
    #region Using statements

    using System;

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;

    using TTengine.Core;
    using TTengine.Comps;

    #endregion

    /// <summary>The system for simulating multiple worlds</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.WorldSystem)]
    public class WorldSystem : EntityComponentProcessingSystem<WorldComp>
    {

        public override void Process(Entity entity, WorldComp worldComp)
        {
            worldComp.World.Update(EntityWorld.Delta);
        }

    }

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.WorldSystemDraw)]
    public class WorldRenderSystem : EntityComponentProcessingSystem<WorldComp>
    {
        public override void Process(Entity entity, WorldComp worldComp)
        {
            worldComp.World.Draw();
        }

    }
}