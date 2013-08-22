using System;
using Artemis.Interface;

namespace TTengine.Comps
{
    public class ShapeComp: IComponent
    {
        public float Radius { get; set; }

        public ShapeComp(float radius)
        {
            this.Radius=radius;
        }

    }
}
