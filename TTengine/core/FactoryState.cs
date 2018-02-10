

using System;
using Artemis;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// Object storing a former TTFactory state. When disposed, it sets the TTFactory back to the stored state.
    /// This can be used in using ( ) { ... } blocks for building to selected screens/worlds temporarily.
    /// </summary>
    public class FactoryState : IDisposable
    {
        private EntityWorld buildWorld;
        private ScreenComp buildScreen;
        private TTFactory Factory;

        internal FactoryState(TTFactory factory, EntityWorld buildWorld, ScreenComp buildScreen)
        {
            this.Factory = factory;
            this.buildWorld = buildWorld;
            this.buildScreen = buildScreen;
        }

        public void Dispose()
        {
            if (Factory != null)
            {
                Factory.BuildTo(this.buildWorld);
                Factory.BuildTo(this.buildScreen);
            }
        }
    }
}
