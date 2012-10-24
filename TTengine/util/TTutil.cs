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
        public static void Round(ref Vector2 input)
        {
            input.X = (float)Math.Round(input.X);
            input.Y = (float)Math.Round(input.Y);
        }

        public static int LineCount(string s)
        {
            if (s.Length == 0)
                return 0;
            int result = 1;
            foreach (char c in s)
            {
                if (c.Equals('\n'))
                {
                    result++;
                }
            }
            return result;
        }

    }
}
