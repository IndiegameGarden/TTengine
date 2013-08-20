using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;

namespace TTengine.Systems
{
    public class ScaleSystem: Sys
    {
        public ScaleSystem()
            : base(typeof(ScaleComp))
        {
        }
    }
}
