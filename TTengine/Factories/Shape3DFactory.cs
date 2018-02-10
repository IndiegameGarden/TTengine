using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Artemis;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Factories.Shape3DFactoryItems;

namespace TTengine.Factories
{
    class Shape3DFactory: TTFactory
    {
        public Entity CreateSphere(Entity e)
        {
            var gc = new GeomComp(new SpherePrimitive(BuildScreen.SpriteBatch.GraphicsDevice));
            e.AddC(gc);
            return e;
        }
    }
}
