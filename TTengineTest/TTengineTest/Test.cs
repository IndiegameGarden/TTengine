using System;
using Microsoft.Xna.Framework;
using Artemis;
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
        public Color BackgroundColor = Color.White;

        /// <summary>The Channel onto which this Test will render</summary>
        public Entity Channel;

        protected TestFactory Factory;

        /// <summary>
        /// Create the entities for this specific test
        /// </summary>
        public abstract void Create();

        /// <summary>
        /// set Factory building output to another screen, world or channel
        /// <seealso cref="BuildToDefault"/>
        /// </summary>
        /// <param name="screen"></param>
        public void BuildTo(Entity screen)
        {
            TestFactory.BuildTo(screen);
        }

        /// <summary>
        /// restore Factory building to default screen that was made for this test
        /// </summary>
        public void BuildToDefault()
        {
            TestFactory.BuildTo(Channel);
        }
    }
}
