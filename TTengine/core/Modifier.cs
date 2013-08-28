using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;

namespace TTengine.Core
{
    public class Modifier
    {
        public delegate void ModifierDelegate(Entity entity);

        public ModifierDelegate Code { get; private set; }

        public Modifier(ModifierDelegate code)
        {
            this.Code = code;
        }

        public void Execute(Entity contextEntity)
        {
            Code(contextEntity);
        }

    }
}
