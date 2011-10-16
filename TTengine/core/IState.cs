// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine
{
    /**
     * interface that a class functioning as a State must have. State design pattern.
     * Default implementation by the State class.
     */
    public interface IState
    {
        /// Called on entry of the state
        void OnEntry(Gamelet g);

        /// Called on exit of the state
        void OnExit(Gamelet g);

        /// Called every gamelet update cycle
        void OnUpdate(Gamelet g);
    }
}
