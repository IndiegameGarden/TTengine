
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using TreeSharp;
using Artemis;
using TTengine.Core;
using TTengine.Comps;

using PXengine.Core;
using PXengine.Comps;

namespace PXengine.Behaviors
{
    /// <summary>
    /// lets a ThingComp chase another ThingComp when it's visible.
    /// </summary>
    public class ChaseBehavior: Behavior
    {
        /// <summary>followed target of this chase behavior</summary>
        public Entity ChaseTarget;

        /// <summary>chase range in pixels</summary>
        public float ChaseRange = 10.0f;

        /// <summary>range reached when chaser is satisfied and stops chasing (0 means chase all the way)</summary>
        public float SatisfiedRange = 0f;

        public ChaseBehavior(Entity chaseTarget)
        {
            this.ChaseTarget = chaseTarget;
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            var ctx = context as BTAIContext;
            var tc = ctx.Entity.GetComponent<ThingComp>();
            var pc = ctx.Entity.GetComponent<PositionComp>();

            if (ChaseTarget != null && ChaseTarget.IsActive)
            {
                var targetTc = ChaseTarget.GetComponent<ThingComp>();
                var targetPc = ChaseTarget.GetComponent<PositionComp>();
                Vector2 dif;
                if (targetTc.Visible)
                {
                    dif = targetPc.Position - pc.Position;
                    float dist = dif.Length();
                    if (dist > 0f && dist <= ChaseRange && dist > SatisfiedRange)
                    {
                        // set control direction towards chase-target
                        var tcc = ctx.Entity.GetComponent<ControlComp>();
                        tcc.Move = dif;
                        yield return RunStatus.Success;
                    }
                }
            }
            yield return RunStatus.Failure;
        }

    }
}
