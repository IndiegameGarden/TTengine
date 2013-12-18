using System;
using Microsoft.Xna.Framework;

using TTengine.Core;

namespace TTengineTest
{
    /// <summary>
    /// Template class for a TTengine test
    /// </summary>
    public abstract class Test
    {
        public Test()
        {
            this.Factory = TestFactory.Instance;
        }

        /// <summary>default background color for this test</summary>
        public Color BackgroundColor = Color.Black;

        protected TestFactory Factory;

        /// <summary>
        /// Create the entities for this test
        /// </summary>
        public abstract void Create();

    }
}
