// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Component for a rectangle-only sprite; i.e. a single pixel texture of a color that's being stretched onto
    /// a given Rectangle.
    /// </summary>
    public class SpriteRectComp : IComponent
    {
        /// <summary>
        /// Width of rectangle or 0 for screen-filling rectangle.
        /// </summary>
        public int Width = 0;

        /// <summary>
        /// Height of rectangle or 0 for screen-filling rectangle.
        /// </summary>
        public int Height = 0;

        public SpriteRectComp()
        {
            ;
        }

    }
}
