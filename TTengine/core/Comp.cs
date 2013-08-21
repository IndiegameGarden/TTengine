// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// A component that can be attached to a Gamelet, providing specific behavior/functions
    /// </summary>
    public abstract class Comp
    {
        public Comp()
        {
            CreateID();
            Register(this);
        }

        /// <summary>
        /// get the unique ID of this object
        /// </summary>
        public uint ID { get { return _ID; } }

        //public abstract uint TypeID { get { return _typeID; } }

        /// <summary>
        /// the parent Gamelet (entity) that this Complet (component) is attached to, or null if none
        /// </summary>
        public Gamelet Parent = null;

        /// <summary>
        /// flag to set processing of this Complet to on (Active=true) or off.
        /// </summary>
        public bool Active = true;

        protected void Register(Comp compToAdd)
        {
            Type t = compToAdd.GetType();
            CompsDict[t].Add(compToAdd);
        }

        protected void UnRegister(Comp compToRemove)
        {
            Type t = compToRemove.GetType();
            CompsDict[t].Remove(compToRemove);
        }

        /// <summary>
        /// Find the first Comp of same type as this in parent gamelet.
        /// </summary>
        /// <returns>Comp instance found in parent Gamelet, or null if none found</returns>
        protected Comp FindParentComp()
        {
            if (Parent == null || Parent.Parent == null)
                return null;
            return Parent.Parent.FindComp(this.GetType());
        }

        #region Internal Vars and methods

        internal static Dictionary<Type,List<Comp>> CompsDict = new Dictionary<Type,List<Comp>>();

        private uint _ID = 0;
        //protected static uint typeID = 0;
        private static uint _IDcounter = 1000000000; // offset to differ from Gamelet ID

        private void CreateID()
        {
            // set my unique id
            _ID = _IDcounter;
            _IDcounter++;
        }

        #endregion

    }
}
