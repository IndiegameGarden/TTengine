// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using TTengine.Core;
using TTengine.Factories.Shape3DFactoryItems;

namespace TTengine.Comps
{
    /// <summary>
    /// Component for rendering 3D geometric shapes
    /// </summary>
    public class GeomComp : Comp
    {
        public GeomComp(GeometricPrimitive g)
        {
            this.Geom = g;
        }

        public GeometricPrimitive Geom;
       
    }
}
