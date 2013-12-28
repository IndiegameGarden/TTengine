using System;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Modifiers
{
    public class VectorModifier<T> where T : IEquatable<T> , IUpdate
    {
        public VectorModifier(T vectorToModify) 
        {
            this.vectorToModify = vectorToModify;
        }

        /// <summary>Whether this Modifier is currently active. Only active modifiers do something.</summary>
        public bool IsActive = true;

        public Vector3 Value;
        public Vector3 Target;
        public double Speed = 0;
        protected T vectorToModify;

        public void OnUpdate(double dt, double simTime)
        {
            if (!IsActive)
                return;

            // scaling logic towards target
            if (Speed > 0)
            {
                Vector3 v = Target - Value;
                if (v.LengthSquared() > 0)
                {
                    Vector3 vm = v; // copy; vm is movement vector to apply
                    vm.Normalize();
                    vm *= (float)(Speed * dt);
                    if (vm.LengthSquared() > v.LengthSquared())
                    {
                        // target reached
                        Value = Target;
                    }
                    else
                    {
                        // Apply motion towards target
                        Value += vm;
                    }
                }
                //FIXME ((Vector3)vectorToModify) = Vector3.Zero;
            }
        }

    }
}
