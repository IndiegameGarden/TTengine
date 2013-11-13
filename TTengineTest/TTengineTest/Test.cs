using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace TTengineTest
{
    /// <summary>
    /// Template class for a TTengine test
    /// </summary>
    public abstract class Test
    {

        /// <summary>default background color for this test</summary>
        public Color BackgroundColor = Color.Black;

        protected TestFactory Factory;

        /// <summary>
        /// Called by test initializer
        /// </summary>
        /// <param name="factory"></param>
        public void Initialize(TestFactory factory)
        {
            this.Factory = factory;
        }

        /// <summary>
        /// Create the entities for this test
        /// </summary>
        public abstract void Create();

    }
}
