// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿
using System;
﻿using System.Reflection;
using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengine.Modifiers
{

    /**
     * attach to a Gamelet to modify a selected arbitrary parameter based on an arbitrary function of time
     */
    public class MyFuncyModifier : Gamelet
    {
        ValueModifier action;
        String propertyName;
        PropertyInfo property;
        FieldInfo field;

        /// <summary>
        /// construct a Modifier that starts immediately and continues indefinitely
        /// </summary>
        /// <param name="action">piece of code (delegate) that calculates the value as a function of SimTime</param>
        /// <param name="property">name of property e.g. "Scale" to modify in the Parent Gamelet</param>
        public MyFuncyModifier(ValueModifier action, String property)
            : base()
        {
            this.action = action;
            this.propertyName = property;
        }

        /// <summary>
        /// construct a Modifier that starts at a given time and then continues indefinitely
        /// </summary>
        /// <param name="action">piece of code (delegate) that calculates the value as a function of SimTime</param>
        /// <param name="property">name of property e.g. "Scale" to modify in the Parent Gamelet</param>
        /// <param name="startTime">start time where 0 is creation time</param>
        public MyFuncyModifier(ValueModifier action, String property, float startTime)
            : base()
        {
            this.action = action;
            this.propertyName = property;
            this.StartTime = startTime;
        }

        /// <summary>
        /// construct a Modifier that starts at a given time and then continues for a specified time.
        /// </summary>
        /// <param name="action">piece of code (delegate) that calculates the value as a function of SimTime</param>
        /// <param name="property">name of property e.g. "Scale" to modify in the Parent Gamelet</param>
        /// <param name="startTime">start time where 0 is creation time</param>
        /// <param name="duration">duration time of the Modifier. After that it deletes itself.</param>
        public MyFuncyModifier(ValueModifier action, String property, float startTime, float duration)
            : base()
        {
            this.action = action;
            this.propertyName = property;
            this.Duration = duration;
            this.StartTime = startTime;
        }

        protected override void OnNewParent()
        {
            base.OnNewParent();
            property = Parent.GetType().GetProperty(propertyName);
            if (property == null)
            {
                field = Parent.GetType().GetField(propertyName);
            }
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            if (Parent != null)
            {
                if (property != null)
                    property.SetValue(Parent,action(SimTime),null);
                if (field != null)
                    field.SetValue(Parent, action(SimTime));
            }
        }
    }
}
