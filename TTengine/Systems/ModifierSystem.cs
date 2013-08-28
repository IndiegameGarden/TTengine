using System;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using TTengine.Comps;
using TTengine.Core;

namespace TTengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 3)]
    public class ModifierSystem: EntityComponentProcessingSystem<ModifierComp>
    {
        public override void Process(Artemis.Entity entity, ModifierComp modComp)
        {
            // iterate the modifiers, and applying each
            foreach (Modifier m in modComp.ModsList)
            {
                m.Execute(entity);
            }

        }
    }
}
