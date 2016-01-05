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
            var oldScr = TTGame.Instance.DrawScreen;    // save state
            ScreenComp sc = null;
            // if this World is part of a Channel...
            if (entity.HasComponent<ScreenComp>())
            {
                // ... make sure it is drawn to the related Screen
                sc = entity.GetComponent<ScreenComp>();
                TTGame.Instance.DrawScreen = sc;
            }
            worldComp.World.Draw();
            TTGame.Instance.DrawScreen = oldScr; // restore state
        }

        public override void OnEnabled(Entity entity)
        {
            base.OnEnabled(entity);
        }

        public override void OnDisabled(Entity entity)
        {
            base.OnDisabled(entity);
        }

    }
}