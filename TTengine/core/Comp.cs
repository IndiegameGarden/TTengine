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
    public class Comp
    {
        public Comp()
        {
            CreateID();
        }

        /// <summary>
        /// get the unique ID of this object
        /// </summary>
        public int ID { get { return _ID; } }

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

        #region Internal Vars and methods

        internal static Dictionary<Type,List<Comp>> CompsDict = new Dictionary<Type,List<Comp>>();

        private int _ID = -1;
        private static int _IDcounter = 1000000000; // offset to differ from Gamelet ID

        private void CreateID()
        {
            // set my unique id
            _ID = _IDcounter;
            _IDcounter++;
        }

        #endregion

    }
}
