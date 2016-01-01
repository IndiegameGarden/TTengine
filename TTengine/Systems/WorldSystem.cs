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
            worldComp.World.Update( new TimeSpan((long) ((double)EntityWorld.Delta * worldComp.TimeWarp)));
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

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.WorldSystemDraw)]
    public class WorldRenderSystem : EntityComponentProcessingSystem<WorldComp>
    {
        public override void Process(Entity entity, WorldComp worldComp)
        {
            var saveOld = TTGame.Instance.DrawScreen;
            TTGame.Instance.DrawScreen = entity.GetComponent<ScreenComp>();
            worldComp.World.Draw();
            TTGame.Instance.DrawScreen = saveOld;
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