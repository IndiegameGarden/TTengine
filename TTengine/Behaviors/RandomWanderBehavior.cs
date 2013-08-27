using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TreeSharp;
using Artemis;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;

namespace TTengine.Behaviors
{
    public class RandomWanderBehavior : TreeNode
    {

        public RandomWanderBehavior(double minDirectionChangeTime, double maxDirectionChangeTime)
        {
            this.MinDirectionChangeTime = minDirectionChangeTime;
            this.MaxDirectionChangeTime = maxDirectionChangeTime;
        }

        public Vector2 CurrentDirection = Vector2.Zero;

        /**
         * the random interval during which the direction changes at a random time. Can be
         * tweaked during operation.
         */
        public double MaxDirectionChangeTime, MinDirectionChangeTime;

        double dirChangeTime = 0f;
        double timeSinceLastChange = 0f;
        
        public override IEnumerable<RunStatus> Execute(object context)
        {
            UpdateParams p = context as UpdateParams;

            // time keeping
            timeSinceLastChange += p.Dt;

            // direction changing
            if (timeSinceLastChange >= dirChangeTime)
            {
                timeSinceLastChange = 0f;
                // TODO: define a double functino also
                dirChangeTime = (double) RandomMath.RandomBetween((float)MinDirectionChangeTime, (float)MaxDirectionChangeTime);
                // TODO: length-preservation in VelocityComp
                Vector2 v = p.Entity.GetComponent<VelocityComp>().Velocity;
                CurrentDirection = RandomMath.RandomDirection() * v.Length();                
                p.Entity.GetComponent<VelocityComp>().Velocity = CurrentDirection;
            }

            yield return RunStatus.Success;
        }

    }
}
