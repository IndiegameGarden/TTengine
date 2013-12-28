using System;
using Microsoft.Xna.Framework;

using Artemis;
using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// A Modifier that can be set with current Value and Target, where Value will move towards
    /// Target every update. A custom code block will receive the Value and can use this to 
    /// modify the variables/properties of any object.
    /// </summary>
    /// <typeparam name="T">The type of object to be modified in the custom code block</typeparam>
    public class TargetModifier<T> : IUpdate
    {

        /// <summary>
        /// Create a new Modifier that can modify an object of specified type T
        /// </summary>
        /// <param name="code">Code (method or delegate block) to execute, must have 'void method(T obj, Vector3 value)' signature</param>
        /// <param name="objectToModify">The object to modify in the code</param>
        public TargetModifier(ModifierDelegate code, T objectToModify)
        {
            this.ModifierCode = code;
            this.objectToModify = objectToModify;
        }

        /// <summary>Modifier delegate, i.e. the code (method) signature of the custom code block</summary>
        public delegate void ModifierDelegate(T mod, Vector3 value);

        protected ModifierDelegate ModifierCode { get; private set; }

        /// <summary>Whether this Modifier is currently active. Only active modifiers do something.</summary>
        public bool IsActive = true;

        public Vector3 Value;
        
        public Vector3 Target;

        public double Speed = 0;

        // internal storage of object to modify
        // Entity not needed to store: this is passed as context at runtime.
        private T objectToModify;

        public void AttachTo(Entity e)
        {
            if (!e.HasComponent<ScriptComp>())
                e.AddComponent(new ScriptComp());
            e.GetComponent<ScriptComp>().Add(this);
        }

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
                ModifierCode(objectToModify, Value);
            }
        }

    }
}
