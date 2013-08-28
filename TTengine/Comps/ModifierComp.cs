using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using TTengine.Core;

namespace TTengine.Comps
{
    public class ModifierComp: IComponent
    {
        public List<Modifier> ModsList = new List<Modifier>();

        public void Add(Modifier mod) 
        {
            ModsList.Add(mod);
        }
    }
}
