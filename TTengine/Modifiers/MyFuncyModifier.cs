// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTengine.Core;

namespace TTengine.Modifiers
{
    /**
     * a Modifier that can modify a float field of a Gamelet with an arbitrary user function, which
     * is given by a delegate of type ValueModifier.
     */
    public class MyFuncyModifier: Modifier
    {
        ValueModifier action;

        /// <summary>
        /// construct a Modifier that modifies the named property or field in this.Parent
        /// </summary>
        /// <param name="action">piece of code (delegate) that calculates the value as a function of SimTime</param>
        /// <param name="propertyOrField">name of float type propertyOrField or field e.g. "Scale" to modify in the Parent Gamelet</param>
        public MyFuncyModifier(ValueModifier action, String propertyOrField)
            : base(propertyOrField)
        {
            this.action = action;
        }

        /// <summary>
        /// construct a Modifier that modifies the named property or field 'subPropertyOrField' in
        /// this.Parent.'property'
        /// </summary>
        /// <param name="action"></param>
        /// <param name="property">name of a property object of the Parent in which 'propertyOrField' is contained.
        ///                        For example, "Motion". </param>
        /// <param name="propertyOrField">name of a float type property or field to modify in the above indicated property object.
        ///                        For example, "RotateModifier".</param>
        public MyFuncyModifier(ValueModifier action, String property, String subPropertyOrField)
            : base(property,subPropertyOrField)
        {
            this.action = action;
        }

        protected override float ModifierValue(ref UpdateParams p)
        {
            return action(SimTime);
        }
    }
}
