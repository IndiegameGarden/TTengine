using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.System;
using Artemis.Manager;
using Artemis.Attributes;
using PXengine.Comps;
using TTengine.Comps;
using TTengine.Util;

namespace PXengine.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 3)]
    public class ThingControlSystem : EntityComponentProcessingSystem<ThingComp,ControlComp,TargetMotionComp>
    {
        public override void Process(Entity entity, ThingComp tc, ControlComp cc, TargetMotionComp targetPosComp)
        {
            cc.TimeBeforeNextMove -= Dt;

            // check how much push I get. If get pushed, try to move
            if (cc.PushFromOthers.LengthSquared() > cc.PushForce)
            {
                cc.Move = cc.PushFromOthers;
                cc.Move.Normalize();
            }
            else
            {
                // if not yet time to make my move, return
                if (cc.TimeBeforeNextMove > 0)
                    return;
            }

            // reset the countdown timer back to its value
            if (cc.TimeBeforeNextMove <= 0)
                cc.TimeBeforeNextMove += cc.TimeBetweenMoves;

            // if no move to make, return
            if (cc.Move.LengthSquared() == 0f)
                return;

            // compute new facingDirection from final TargetMove
            tc.FacingDirection = cc.Move;
            tc.FacingDirection.Normalize();

            // check if passable...
            // FIXME
            /*
            List<Entity> cols = tc.DetectCollisions(entity,tcc.TargetMove);
            if (!tc.IsCollisionFree && cols.Count > 0 && tcc.PushForce > 0f)
            {
                // no - so try to push neighbouring things away
                foreach (Entity t in cols)
                {
                    if (t.HasComponent<ControlComp>())
                    {
                        t.GetComponent<ControlComp>().BePushed(tcc.TargetMove, tcc.PushForce);
                    }
                }
            }

            if (tc.IsCollisionFree || (!tc.CollidesWithBackground(tcc.TargetMove) && cols.Count == 0))
            {
                // yes - passable
                tc.Target += tcc.TargetMove;
                TTutil.Round(ref tc.Target);
            }
             */

            targetPosComp.Target += cc.Move;
        }
    }
}
