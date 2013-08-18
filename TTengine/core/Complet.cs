using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// A component that can be attached to a Gamelet, providing specific behavior/functions
    /// </summary>
    class Complet
    {
        public Complet()
        {
            CreateID();
        }

        #region Properties

        /// <summary>
        /// get the unique ID of this object
        /// </summary>
        public int ID { get { return _ID; } }

        public Gamelet Parent;

        #endregion


        #region Internal Vars and methods

        private int _ID = -1;
        private static int _IDcounter = 0;

        private void CreateID()
        {
            // set my unique id
            _ID = _IDcounter;
            _IDcounter++;
        }

        #endregion

    }
}
