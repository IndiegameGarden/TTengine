using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TTengine.Util
{
    /**
     * general utility methods that are handy in creating your games
     */
    public class TTutil
    {
        /// <summary>
        /// Rounds the components of Vector2 in-place
        /// </summary>
        /// <param name="input">Vector2 to round</param>
        public static void Round(Vector2 input)
        {
            input.X = (float)Math.Round((float)input.X);
            input.Y = (float)Math.Round((float)input.Y);
        }

    }
}
