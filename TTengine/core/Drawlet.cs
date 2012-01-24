using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /**
     * a Gamelet that can move and draw
     */
    public class Drawlet: Gamelet
    {
        public Drawlet()
        {
            Motion = new Motion();
            Add(Motion);
            DrawInfo = new DrawInfo();            
            Add(DrawInfo);
        }
    }
}
