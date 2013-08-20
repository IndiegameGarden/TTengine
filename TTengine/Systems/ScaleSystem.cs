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

        public override void UpdateComp(Comp c, UpdateParams p)
        {
            ScaleComp sc = c as ScaleComp;

            // scaling logic towards target
            if (sc.ScaleSpeed > 0)
            {
                if (sc.Scale < sc.ScaleTarget)
                {
                    sc.Scale += sc.ScaleSpeed * (sc.ScaleTarget - sc.Scale); 
                    if (sc.Scale > sc.ScaleTarget)
                    {
                        sc.Scale = sc.ScaleTarget;
                    }
                }
                else if (sc.Scale > sc.ScaleTarget)
                {
                    sc.Scale += sc.ScaleSpeed * (sc.ScaleTarget - sc.Scale); 
                    if (sc.Scale < sc.ScaleTarget)
                    {
                        sc.Scale = sc.ScaleTarget;
                    }
                }
            }

        }

    }
}
