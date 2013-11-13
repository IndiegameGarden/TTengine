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
    /// A 'quick script' that can be configured with a custom code block, intended to modify
    /// a certain parameter of another object of type T. 
    /// </summary>
    public class Modifier<T>: IScript
    {
        /// <summary>Modifier delegate, i.e. the code (method) signature of the custom code block</summary>
        public delegate void ModifierDelegate(T mod, double value);

        /// <summary>Whether this Modifier is currently active. Only active modifiers do something.</summary>
        public bool IsActive = true;

        // class stores code for one of different types of modifier. FIXME split in different classes
        //protected ModifyEntityDelegate  ModifyEntityCode { get; private set; }
        protected ModifierDelegate ModifierCode { get; private set; }
        //protected ModifyCompDelegate    ModifyCompCode { get; private set; }

        // internal storage of object to modify
        // Entity not needed to store: this is passed as context at runtime.
        private T objectToModify;
        //private IComponent compToModify;

        /// <summary>
        /// Create a new Modifier that can modify an object of specified type T
        /// </summary>
        /// <param name="code">Code (method or delegate block) to execute, must have 'void method(T obj, double value)' signature</param>
        public Modifier(ModifierDelegate code, T objectToModify)
        {
            //this.Type = ModifierType.MODIFIER_MODIFIER;
            this.ModifierCode = code;
            this.objectToModify = objectToModify;
        }

        public void AttachTo(Entity e)
        {
            if (!e.HasComponent<ScriptComp>())
                e.AddComponent(new ScriptComp());
            e.GetComponent<ScriptComp>().Add(this);
        }

        public void OnUpdate(ScriptContext ctx)
        {
            if (IsActive)
            {
                double value = GetValue(ctx.ScriptComp.SimTime);
                ModifierCode(objectToModify, value);
            }
        }

        public void OnDraw(ScriptContext ctx)
        {
        }

        /// <summary>
        /// Get a value to pass to a Modifier as a function of time. Value is to be used
        /// by the custom code (delegate) that is set in this Modifier, in some way, to
        /// modify something. This can be overridden by subclasses to generate other kinds
        /// of 'signals' e.g. sine wave, square wave, triangle wave, random, etc.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns>value as function of time (specified by subclass of Modifier)</returns>
        protected virtual double GetValue(double time)
        {
            return time;
        }

    }
}
