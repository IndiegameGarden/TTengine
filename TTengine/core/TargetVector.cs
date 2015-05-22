using System;
using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /// <summary>
    /// A Vector3 (Current) that progresses towards a set Target with a set Speed
    /// TODO add modes for different motion patterns (linear, rel, fly, etc)
    /// </summary>
    public class TargetVector: IUpdate
    {
        /// <summary>
        /// Create with Vector3.Zero initial values
        /// </summary>
        public TargetVector()
        {
        }

        /// <summary>
        /// Create with given initial values
        /// </summary>
        /// <param name="initialValue">Value for Target and Current</param>
        public TargetVector(Vector2 initialValue)
        {
            Target = initialValue;
            Current = initialValue;
        }

        public bool     IsActive = true;
        public Vector2  Target  = Vector2.Zero;
        public Vector2  Current = Vector2.Zero;
        public double   Speed   = 10;

        public void AddToTarget(Vector2 v)
        {
            Target += v;
        }

        public Vector2 Target2D
        {
            get { return new Vector2(Target.X, Target.Y); }
            set
            {
                Target.X = value.X;
                Target.Y = value.Y;
            }
        }

        public void OnUpdate(double dt, double simTime = 0)
        {
            if (!IsActive)
                return;
            var v = Target - Current;
            if (v.LengthSquared() > 0)
            {
                var vm = v;
                vm.Normalize();
                vm *= (float)(Speed * dt);
                if (vm.LengthSquared() > v.LengthSquared())
                {
                    // target reached
                    Current = Target;
                }
                else
                {
                    Current += vm;
                }
            }
        }

    }
}
