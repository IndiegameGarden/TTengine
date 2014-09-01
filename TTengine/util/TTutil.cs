using System;
using System.IO;
using System.Collections.Generic;

using System.Text;
using Microsoft.Xna.Framework;

namespace TTengine.Util
{
    /**
     * general utility methods that are handy in creating your games
     */
    public class TTUtil
    {
        /// <summary>
        /// Rounds the components of a Vector2 in-place
        /// </summary>
        /// <param name="input">Vector2 to round</param>
        public static void Round(ref Vector2 input)
        {
            input.X = (float)Math.Round(input.X);
            input.Y = (float)Math.Round(input.Y);
        }

        /// <summary>
        /// Count the number of lines in a string
        /// </summary>
        /// <param name="s">The string</param>
        /// <returns>The linecount, or 0 if empty string</returns>
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

        /// <summary>
        /// Invert color
        /// </summary>
        /// <param name="c">color to invert</param>
        /// <returns>Inverted color, "1-c"</returns>
        public static Color InvertColor(Color c)
        {
            return new Color(new Vector3(1f, 1f, 1f) - c.ToVector3());
        }
    }
}
