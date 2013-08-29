using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;

namespace TTengine.Core
{
    public class Modifier
    {
        public delegate void ModifyEntityDelegate(Entity entity, double value);
        public delegate void ModifyModifierDelegate(Modifier mod, double value);
        public delegate void ModifyCompDelegate(Comp comp, double value);

        public ModifyEntityDelegate EntityModifier { get; private set; }
        public ModifyModifierDelegate ModifierModifier { get; private set; }
        public ModifyModifierDelegate CompModifier { get; private set; }

        public bool IsActive = true;

        private Modifier modifierToModify;

        public Modifier(ModifyEntityDelegate code)
        {
            this.EntityModifier = code;
        }

        public Modifier(ModifyModifierDelegate code, Modifier modifierToModify)
        {
            this.ModifierModifier = code;
            this.modifierToModify = modifierToModify;
        }

        public void Execute(Entity contextEntity, double time)
        {
            if (IsActive)
            {
                double value = GetValue(time);
                if (EntityModifier != null)
                    EntityModifier(contextEntity,value);
                else if (ModifierModifier != null)
                    ModifierModifier(modifierToModify,value);
            }
        }

        /// <summary>
        /// Get a value to pass to a Modifier as a function of time. Value is to be used
        /// by the custom code (delegate) that is set in this Modifier, in some way, to
        /// modify something.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns></returns>
        protected virtual double GetValue(double time)
        {
            return time;
        }

    }
}
