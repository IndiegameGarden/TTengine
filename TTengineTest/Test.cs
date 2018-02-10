// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;
using Artemis;
using TTengine.Core;

namespace TTengineTest
{
    /// <summary>
    /// Template class for a single TTengine test
    /// </summary>
    public abstract class Test : TestFactory
    {
        /// <summary>default background color for this test</summary>
        public Color BackgroundColor = Color.White;

        /// <summary>font color for this test</summary>
        public Color FontColor = Color.Black;

        /// <summary>The Channel onto which this Test will render</summary>
        public Entity Channel;

        /// <summary>
        /// Create all the entities for this specific test
        /// </summary>
        public abstract void BuildAll();

    }
}
