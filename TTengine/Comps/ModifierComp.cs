using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using TTengine.Modifiers;

namespace TTengine.Comps
{
    public class ModifierComp: IComponent
    {
        public double SimTime = 0;

        public List<Modifier> ModsList = new List<Modifier>();

        public void Add(Modifier mod) 
        {
            ModsList.Add(mod);
        }
    }
}
