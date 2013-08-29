using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;

namespace TTengine.Core
{
    /// <summary>
    /// An object that can be configured with custom code, intended to modify
    /// a certain parameter of another object or IComponent. It can also modify
    /// the composition of an entity.
    /// </summary>
    public class Modifier
    {
        public delegate void ModifyEntityDelegate(Entity entity, double value);
        public delegate void ModifyModifierDelegate(Modifier mod, double value);
        public delegate void ModifyCompDelegate(Comp comp, double value);

        // class stores code for one of different types of modifier. FIXME split in different classes
        protected ModifyEntityDelegate EntityModifierCode { get; private set; }
        protected ModifyModifierDelegate ModifierModifierCode { get; private set; }
        protected ModifyModifierDelegate CompModifierCode { get; private set; }

        /// <summary>Whether this Modifier is currently active. Only active modifiers do something.</summary>
        public bool IsActive = true;

        // internal storage
        private Modifier modifierToModify;

        /// <summary>
        /// Create a new Entity Modifier that can only modify things within a single Entity.
        /// </summary>
        /// <param name="code">Code (method or delegate block) to execute, must have 'void method(Entity e)' signature</param>
        public Modifier(ModifyEntityDelegate code)
        {
            this.EntityModifierCode = code;
        }

        public Modifier(ModifyModifierDelegate code, Modifier modifierToModify)
        {
            this.ModifierModifierCode = code;
            this.modifierToModify = modifierToModify;
        }

        public void Execute(Entity contextEntity, double time)
        {
            if (IsActive)
            {
                double value = GetValue(time);
                if (EntityModifierCode != null)
                    EntityModifierCode(contextEntity,value);
                else if (ModifierModifierCode != null)
                    ModifierModifierCode(modifierToModify,value);
            }
        }

        /// <summary>
        /// Get a value to pass to a Modifier as a function of time. Value is to be used
        /// by the custom code (delegate) that is set in this Modifier, in some way, to
        /// modify something. This can be overridden by subclasses to generate other kinds
        /// of 'signals' e.g. sine wave, square wave, triangle wave, random, etc.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns></returns>
        protected virtual double GetValue(double time)
        {
            return time;
        }

    }
}
