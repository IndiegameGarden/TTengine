using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.Interface;
using TTengine.Core;
using TTengine.Comps;

namespace TTengine.Modifiers
{
    /// <summary>
    /// The available types of modifiers in Modifier. Call the proper constructor to create
    /// one of these types.
    /// </summary>
    public enum ModifierType
    {
        ENTITY_MODIFIER, 
        MODIFIER_MODIFIER,
        COMP_MODIFIER
    }

    /// <summary>
    /// An object that can be configured with custom code, intended to modify
    /// a certain parameter of another object or IComponent. It can also modify
    /// the composition of an entity.
    /// </summary>
    public class Modifier
    {
        public delegate void ModifyEntityDelegate(Entity entity, double value);
        public delegate void ModifyModifierDelegate(Modifier mod, double value);
        public delegate void ModifyCompDelegate(IComponent comp, double value);

        /// <summary>Can be used to check the type of Modifier</summary>
        public ModifierType Type;

        /// <summary>Whether this Modifier is currently active. Only active modifiers do something.</summary>
        public bool IsActive = true;

        // class stores code for one of different types of modifier. FIXME split in different classes
        protected ModifyEntityDelegate  ModifyEntityCode { get; private set; }
        protected ModifyModifierDelegate ModifyModifierCode { get; private set; }
        protected ModifyCompDelegate    ModifyCompCode { get; private set; }

        // internal storage of object to modify
        // Entity not needed to store: this is passed as context at runtime.
        private Modifier modifierToModify;
        private IComponent compToModify;

        /// <summary>
        /// Create a new Entity Modifier that can only modify things within a single Entity.
        /// </summary>
        /// <param name="code">Code (method or delegate block) to execute, must have 'void method(Entity e)' signature</param>
        public Modifier(ModifyEntityDelegate code)
        {
            this.Type = ModifierType.ENTITY_MODIFIER;
            this.ModifyEntityCode = code;
        }

        public Modifier(ModifyModifierDelegate code, Modifier modifierToModify)
        {
            this.Type = ModifierType.MODIFIER_MODIFIER;
            this.ModifyModifierCode = code;
            this.modifierToModify = modifierToModify;
        }

        public Modifier(ModifyCompDelegate code, IComponent compToModify)
        {
            this.Type = ModifierType.COMP_MODIFIER;
            this.ModifyCompCode = code;
            this.compToModify = compToModify;
        }

        public void AttachTo(Entity e)
        {
            if (!e.HasComponent<ModifierComp>())
                e.AddComponent(new ModifierComp());
            e.GetComponent<ModifierComp>().Add(this);
        }

        public void Execute(Entity contextEntity, double time)
        {
            if (IsActive)
            {
                double value = GetValue(time);
                switch (Type)
                {
                    case ModifierType.ENTITY_MODIFIER:
                        ModifyEntityCode(contextEntity, value);
                        break;
                    case ModifierType.MODIFIER_MODIFIER:
                        ModifyModifierCode(modifierToModify, value);
                        break;
                    case ModifierType.COMP_MODIFIER:
                        ModifyCompCode(compToModify, value);
                        break;
                }
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
