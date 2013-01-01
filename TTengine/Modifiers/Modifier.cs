// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿
using System;
﻿using System.Reflection;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{

    /**
     * function template for a delegate, piece of code, that modifies an input value and returns an output value
     */
    public delegate float ValueModifier(float value);


    /**
     * attach to a Gamelet to modify a selected arbitrary parameter based on an arbitrary function of time
     * specified using a delegate.
     */
    public class Modifier : Gamelet
    {
        Object modifiedObject;
        String propertyName, propertyContainerName;
        PropertyInfo property;
        FieldInfo field;

        /// <summary>
        /// construct a Modifier that modifies the named property or field in this.Parent
        /// </summary>
        /// <param name="action">piece of code (delegate) that calculates the value as a function of SimTime</param>
        /// <param name="propertyOrField">name of float type propertyOrField or field e.g. "Scale" to modify in the Parent Gamelet</param>
        public Modifier(String propertyOrField)
            : base()
        {
            this.propertyName = propertyOrField;
        }

        /// <summary>
        /// construct a Modifier that modifies the named property or field 'subPropertyOrField' in
        /// this.Parent.'property'
        /// </summary>
        /// <param name="property">name of a property object of the Parent in which 'propertyOrField' is contained.
        ///                        For example, "Motion". </param>
        /// <param name="propertyOrField">name of a float type property or field to modify in the above indicated property object.
        ///                        For example, "RotateModifier".</param>
        public Modifier(String property, String subPropertyOrField)
            : base()
        {
            this.propertyContainerName = property;
            this.propertyName = subPropertyOrField;
        }

        protected override void OnNewParent()
        {
            base.OnNewParent();
            if (propertyName != null && Parent != null) 
            {
                // try to detect a container, if specified
                if (propertyContainerName != null)
                {
                    FieldInfo f = Parent.GetType().GetField(propertyContainerName);
                    if (f != null)
                        modifiedObject = f.GetValue(Parent);
                    //modifiedObject = Parent.GetType();
                }
                else
                {
                    modifiedObject = Parent;
                }

                // try to detect a propertyOrField or field with given name.
                property = modifiedObject.GetType().GetProperty(propertyName);
                if (property == null)
                {
                    field = modifiedObject.GetType().GetField(propertyName);
                }

                if (field == null)
                {
                    throw new MissingMemberException("Member " + propertyName + " could not be found in " + Parent );
                }

            }
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            if (modifiedObject != null)
            {
                // calculate modifier value and try to update propertyOrField or field with it
                float val = ModifierValue(ref p);
                if (property != null)
                    property.SetValue(modifiedObject, val, null);
                else if (field != null)
                    field.SetValue(modifiedObject, val);
            }
        }

        /// <summary>
        /// generate the value for the modifier on each update. Can be overridden by
        /// subclasses to return other values.
        /// </summary>
        /// <param name="p">The OnUpdate parameters</param>
        /// <returns>calculated modifier value specific to the (sub)class</returns>
        protected virtual float ModifierValue(ref UpdateParams p)
        {
            return SimTime;
        }
    }
}
