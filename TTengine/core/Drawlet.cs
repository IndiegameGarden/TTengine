using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /**
     * a Gamelet that can move, scale, zoom, and draw itself. 
     * Note: a Gamelet with any children
     * that can draw, need to be Drawlet themselves.
     */
    public class Drawlet: Gamelet
    {
        public Drawlet()
        {
            Motion = new Motion();
            DrawInfo = new DrawInfo();            
        }

        protected override void OnNewParent()
        {
            base.OnNewParent();
            Add(Motion);
            Add(DrawInfo);
        }

        internal override void Update(ref UpdateParams p)
        {
            base.Update(ref p);
            if (Active && DrawInfo != null)
            {
                DrawInfo.UpdateSmoothingCache(ref p);
            }
        }
    }
}
