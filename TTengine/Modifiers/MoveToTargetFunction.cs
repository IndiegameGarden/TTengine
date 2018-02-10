using System;
using Microsoft.Xna.Framework;
using Artemis;
using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// An IVectorFunction that can be set with current Value and Target, where Value will move towards
    /// Target every update. 
    /// </summary>
    public class MoveToTargetFunction: IVectorFunction
    {

        public MoveToTargetFunction()
        {
        }

        public Vector3 CurrentValue;
        
        public Vector3 Target;

        public double Speed = 0;

        public Vector3 Value(ScriptComp ctx)
        {
            // scaling logic towards target
            if (Speed > 0)
            {
                Vector3 v = Target - CurrentValue;
                if (v.LengthSquared() > 0)
                {
                    Vector3 vm = v; // copy; vm is movement vector to apply
                    vm.Normalize();
                    vm *= (float)(Speed * ctx.Dt);
                    if (vm.LengthSquared() > v.LengthSquared())
                    {
                        // target reached
                        CurrentValue = Target;
                    }
                    else
                    {
                        // Apply motion towards target
                        CurrentValue += vm;
                    }
                }                
            }
            return CurrentValue;
        }

    }
}
