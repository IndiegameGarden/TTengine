using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using TTengine.Core;
using TTengine.Comps;
using TreeSharp;

namespace TTengine.Systems
{
    /// <summary>
    /// A Behavior Tree (BT) based AI system for Entities
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 2)]
    public class BTAISystem : EntityComponentProcessingSystem<BTAIComp>
    {
        private BTAIContext updParams = new BTAIContext();

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            // once per update-cycle, set timing in updParams
            updParams.Dt = TimeSpan.FromTicks(this.EntityWorld.Delta).TotalSeconds;
            updParams.SimTime += updParams.Dt;
            base.ProcessEntities(entities);
        }

        public override void Process(Entity entity, BTAIComp btComp)
        {
            updParams.Entity = entity;
            updParams.Comp = btComp;

            if (btComp.rootNode.LastStatus == null)
                btComp.rootNode.Start(updParams);
            if (btComp.rootNode.LastStatus == RunStatus.Success)
                btComp.rootNode.Start(updParams);
            // TODO rename UpdateParams/updParams?
            btComp.rootNode.Tick(updParams);

        }

    }
}
